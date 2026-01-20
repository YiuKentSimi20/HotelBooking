# Hotel Booking System - Project Context & Guidelines

This document provides the architectural blueprint, project structure, and coding standards for the Hotel Booking System. **GitHub Copilot must strictly follow these guidelines when generating code.**

---

## 1. System Architecture Overview

The system consists of **3 Bounded Contexts** (Microservices) communicating asynchronously via **Azure Service Bus**.

### Data Flow
1.  **Booking Context** (User-Facing API)
    * Input: REST API (`BookTicketCommand`).
    * Process: Validates, sets price, processes payment.
    * Output: Publishes `BookingPaidEvent`.
2.  **Invoicing Context** (Backend Processor)
    * Input: Consumes `BookingPaidEvent`.
    * Process: Generates Invoice, calculates VAT.
    * Output: Publishes `InvoiceIssuedEvent`.
3.  **Accommodation Context** (Backend Processor)
    * Input: Consumes `InvoiceIssuedEvent`.
    * Process: Assigns physical room, generates access code.
    * Output: Finalizes Check-In (Persists `RoomAssigned` state).

### Persistence
* **Database-per-Service:** Each context has its own isolated SQL Database.
* **Persistence Strategy:** Domain models are mapped to Database Entities in the `Data` layer.

---

## 2. Solution Structure

The solution follows a strict separation of concerns based on the "Examples" reference architecture.

### Global Shared Projects
* **Messaging/**
    * `HotelBooking.Messaging` (Interfaces: `IEventHandler`, `IEventSender`)
    * `HotelBooking.Messaging.ServiceBus` (Implementation using Azure Service Bus)

### Context 1: Booking (API Pattern)
* `HotelBooking.Booking.Dto` (Shared Contracts, Integration Events)
* `HotelBooking.Booking.Domain` (Pure Domain Logic, Functional DDD)
* `HotelBooking.Booking.Data` (EF Core, Repositories)
* `HotelBooking.Booking.Api` (**Entry Point**: REST Controllers)

### Context 2 & 3: Invoicing / Accommodation (Worker Pattern)
* `HotelBooking.{Context}.Dto`
* `HotelBooking.{Context}.Domain`
* `HotelBooking.{Context}.Data`
* `HotelBooking.{Context}.EventProcessor` (**Entry Point**: BackgroundService/Worker)

---

## 3. Internal Project Structure & Namespaces

For every context, the internal folder structure must look exactly like this:

### A. The Domain Project (`.Domain`)
Namespace: `HotelBooking.{Context}.Domain`
* **Models/**
    * `Commands/` (Immutable Records, e.g., `BookRoomCommand`)
    * `ValueObjects/` (Private ctor, `TryParse`, Validation, e.g., `BookingPeriod`)
    * `Entities/` (Disjoint Union Records, e.g., `BookingState`)
    * `Events/` (Domain Events, e.g., `BookingSucceeded`)
* **Operations/** (Classes inheriting `DomainOperation`, pure transformations)
* **Workflows/** (Classes orchestrating operations, ending with `.ToEvent()`)
* **Repositories/** (Interfaces only, e.g., `IBookingRepository`)

### B. The Data Project (`.Data`)
Namespace: `HotelBooking.{Context}.Data`
* **Models/** (Mutable POCOs for EF Core, e.g., `BookingEntity`)
* **Repositories/** (Implementation of Domain Interfaces, Maps Domain <-> Entity)
* `{Context}DbContext.cs`

### C. The Dto Project (`.Dto`)
Namespace: `HotelBooking.{Context}.Dto`
* **Events/** (Classes used for Service Bus messages)
* **Models/** (Shared data structures)

---

## 4. Coding Standards (Functional DDD)

### Rule #1: Entity States (Disjoint Unions)
Do not use a single class with flags. Use a static class containing record types implementing a marker interface.

```csharp
public static class TicketBooking
{
    public interface IBooking { }
    
    // 1. Unvalidated State
    public record UnvalidatedBooking(BookTicketCommand Command) : IBooking;
    
    // 2. Validated State
    public record ValidatedBooking(GuestEmail Email, BookingPeriod Period) : IBooking;
    
    // 3. Invalid State
    public record InvalidBooking(IEnumerable<string> Reasons) : IBooking;
}