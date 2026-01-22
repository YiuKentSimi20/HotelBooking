# FrontDesk Context - Implementation Summary

## Overview
Implementation of the FrontDesk (Accommodation) context that handles guest check-in when a `BookingPaidIntegrationEvent` is received. It validates the booking, generates an access code for the guest, and creates a room assignment.

---

## Database Schema (Hotel_Accommodation_Db)

```sql
-- Rooms table (inventory)
CREATE TABLE Rooms (
    RoomId INT IDENTITY(1,1) PRIMARY KEY,
    RoomNumber NVARCHAR(10) NOT NULL UNIQUE,
    RoomType NVARCHAR(50) NOT NULL,
    IsClean BIT NOT NULL DEFAULT 1,
    IsOutOfService BIT NOT NULL DEFAULT 0
);

-- RoomAssignments table (check-in logic)
CREATE TABLE RoomAssignments (
    AssignmentId INT IDENTITY(1,1) PRIMARY KEY,
    BookingId INT NOT NULL,
    RoomId INT NOT NULL,
    CheckInDate DATETIME2 NOT NULL DEFAULT GETDATE(),
    CheckOutDate DATETIME2 NULL,
    AccessCode NVARCHAR(20) NOT NULL,
    CONSTRAINT FK_Assignments_Rooms FOREIGN KEY (RoomId) REFERENCES Rooms(RoomId)
);
```

---

## Files Created

### FrontDesk.Domain

#### Commands
- **CheckInCommand.cs** - Input command with all booking details

#### Value Objects
- **AccessCode.cs** - 6-digit generated access code for guest
- **RoomNumber.cs** - Validated room number
- **CustomerName.cs** - Validated customer name (2-200 chars)
- **CustomerEmail.cs** - Validated email with regex

#### Entities (CheckIn.cs)
- **ICheckIn** - Base interface
- **UnvalidatedCheckIn** - Initial state with command
- **ValidatedCheckIn** - After validation (includes RoomId)
- **AccessCodeGeneratedCheckIn** - After access code generation
- **CompletedCheckIn** - Final state with AssignmentId
- **InvalidCheckIn** - Error state with reasons

#### Events (CheckInCompletedEvent.cs)
- **ICheckInCompletedEvent** - Base interface
- **CheckInSucceeded** - Success event with all check-in data + AccessCode
- **CheckInFailed** - Failure event with error reasons
- **ToEvent()** - Extension method to convert ICheckIn to event

#### Operations
- **DomainOperation.cs** - Base abstract class
- **CheckInOperation.cs** - CheckIn-specific operation base
- **ValidateCheckInOperation.cs** - Validates data and gets RoomId
- **GenerateAccessCodeOperation.cs** - Generates 6-digit access code

#### Repositories
- **IRoomRepository.cs** - Room operations (GetRoomIdByNumber, IsAvailable, MarkDirty)
- **IRoomAssignmentRepository.cs** - Assignment operations (Save, ExistsForBooking)

#### Workflows
- **CheckInWorkflow.cs** - Orchestrates the check-in process

#### Exceptions
- **DomainException.cs** - Base exception
- **InvalidCheckInStateException.cs** - Invalid state exception

---

### FrontDesk.Data

#### Models
- **RoomEntity.cs** - EF Core entity for Rooms table
- **RoomAssignmentEntity.cs** - EF Core entity for RoomAssignments table

#### DbContext
- **FrontDeskDbContext.cs** - EF Core context with Rooms and RoomAssignments DbSets

#### Repositories
- **RoomRepository.cs** - Implementation of IRoomRepository
- **RoomAssignmentRepository.cs** - Implementation of IRoomAssignmentRepository

---

### FrontDesk.Dto

#### Events
- **BookingPaidIntegrationEvent.cs** - Incoming event from Booking context
- **CheckInCompletedIntegrationEvent.cs** - Outgoing event to downstream consumers

---

### FrontDesk.Accommodation.EventProcessor

- **BookingPaidIntegrationEventHandler.cs** - Handles incoming events, executes workflow, publishes result
- **Program.cs** - DI configuration and service registration
- **Worker.cs** - Background worker for event listening

---

## Workflow Flow

```
BookingPaidIntegrationEvent (from "bookings" topic)
    ↓
CheckInCommand
    ↓
UnvalidatedCheckIn
    ↓ ValidateCheckInOperation (get RoomId, check availability)
ValidatedCheckIn (includes RoomId)
    ↓ GenerateAccessCodeOperation
AccessCodeGeneratedCheckIn (includes AccessCode)
    ↓ SaveRoomAssignmentAsync
    ↓ MarkRoomAsDirtyAsync
CompletedCheckIn (includes AssignmentId)
    ↓
CheckInCompletedIntegrationEvent → "checkins" topic
```

---

## Access Code Generation

```csharp
public static AccessCode Generate()
{
    var random = new Random();
    var code = random.Next(100000, 999999).ToString();
    return new AccessCode(code);
}

// Example: "482715"
```

---

## Room Status Updates

When a guest checks in:
1. **RoomAssignment created** - Links BookingId to RoomId with AccessCode
2. **Room marked as dirty** - `IsClean = false` (will need cleaning after checkout)

---

## Idempotency

The workflow checks if an assignment already exists for a booking:

```csharp
var exists = await assignmentRepository.AssignmentExistsForBookingAsync(command.BookingId);
if (exists)
{
    return new CheckInCompletedEvent.CheckInFailed(
        new[] { $"Room assignment already exists for booking {command.BookingId}" });
}
```

---

## Event Flow

```
Booking.Api
    ↓ BookingPaidIntegrationEvent → "bookings" topic
    ↓
┌───────────────────────────────────────────────────────┐
│                                                       │
│  Invoicing.Accommodation.EventProcessor               │
│  - Generates invoice                                  │
│  - Publishes InvoiceIssuedIntegrationEvent            │
│                                                       │
└───────────────────────────────────────────────────────┘
    ↓
┌───────────────────────────────────────────────────────┐
│                                                       │
│  FrontDesk.Accommodation.EventProcessor               │
│  - Performs check-in                                  │
│  - Generates access code                              │
│  - Creates room assignment                            │
│  - Publishes CheckInCompletedIntegrationEvent         │
│                                                       │
└───────────────────────────────────────────────────────┘
    ↓ CheckInCompletedIntegrationEvent → "checkins" topic
```

---

## Service Registration (Program.cs)

```csharp
// Database
services.AddDbContext<FrontDeskDbContext>(options => 
    options.UseSqlServer(connectionString));

// Repositories
services.AddScoped<IRoomRepository, RoomRepository>();
services.AddScoped<IRoomAssignmentRepository, RoomAssignmentRepository>();

// Workflows
services.AddScoped<CheckInWorkflow>();

// Event handling
services.AddSingleton<IEventSender, ServiceBusTopicEventSender>();
services.AddSingleton<IEventListener, ServiceBusTopicEventListener>();
services.AddScoped<IEventHandler, BookingPaidIntegrationEventHandler>();
```

---

## Example Flow

### Input (BookingPaidIntegrationEvent):
```json
{
  "BookingId": 1,
  "CustomerName": "John Doe",
  "CustomerEmail": "john@example.com",
  "RoomType": "Single",
  "RoomNumber": "101",
  "CheckInDate": "2026-02-10",
  "CheckOutDate": "2026-02-15",
  "TotalAmount": 500.00,
  "PaymentTransactionId": "TXN-ABCD1234"
}
```

### Execution:

1. **ValidateCheckInOperation**
   - Validates all fields ✓
   - Gets RoomId for room "101" → RoomId = 1
   - Checks room is available (not out of service) ✓
   → `ValidatedCheckIn { RoomId = 1 }`

2. **GenerateAccessCodeOperation**
   - Generates 6-digit code: "482715"
   → `AccessCodeGeneratedCheckIn { AccessCode = "482715" }`

3. **Workflow**
   - Saves RoomAssignment → AssignmentId = 1
   - Marks room as dirty (IsClean = false)
   → `CompletedCheckIn { AssignmentId = 1 }`

### Output (CheckInCompletedIntegrationEvent):
```json
{
  "AssignmentId": 1,
  "BookingId": 1,
  "CustomerName": "John Doe",
  "CustomerEmail": "john@example.com",
  "RoomType": "Single",
  "RoomNumber": "101",
  "CheckInDate": "2026-02-10",
  "CheckOutDate": "2026-02-15",
  "RoomId": 1,
  "AccessCode": "482715",
  "Timestamp": "2026-01-22T14:32:56Z"
}
```

---

## Summary

✅ **Domain Layer Complete:**
- 4 Value Objects (AccessCode, RoomNumber, CustomerName, CustomerEmail)
- 1 Command (CheckInCommand)
- 5 Entity States (Unvalidated → Validated → AccessCodeGenerated → Completed, Invalid)
- 2 Event Types (CheckInSucceeded, CheckInFailed)
- 3 Operations (CheckInOperation base, Validate, GenerateAccessCode)
- 1 Workflow (CheckInWorkflow)
- 2 Repository Interfaces (IRoomRepository, IRoomAssignmentRepository)
- 2 Exceptions

✅ **Data Layer Complete:**
- 2 Entities (RoomEntity, RoomAssignmentEntity)
- 1 DbContext (FrontDeskDbContext)
- 2 Repository Implementations

✅ **DTO Layer Complete:**
- 1 Incoming Event (BookingPaidIntegrationEvent)
- 1 Outgoing Event (CheckInCompletedIntegrationEvent)

✅ **Event Processor Complete:**
- Event Handler with workflow execution
- Service Bus integration (receive from "bookings", send to "checkins")
- Program.cs with DI configuration

The FrontDesk context is now ready to process booking events and perform guest check-in!
