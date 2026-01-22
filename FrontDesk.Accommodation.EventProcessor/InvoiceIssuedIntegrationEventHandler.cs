using Example.Events.Models;
using FrontDesk.Domain.Models.Commands;
using FrontDesk.Domain.Models.Events;
using FrontDesk.Domain.Repositories;
using FrontDesk.Domain.Workflows;
using FrontDesk.Dto.Events;
using HotelBooking.Events;

namespace FrontDesk.Accommodation.EventProcessor;

internal class InvoiceIssuedIntegrationEventHandler : AbstractEventHandler<InvoiceIssuedIntegrationEvent>
{
    private readonly CheckInWorkflow _workflow;
    private readonly IRoomRepository _roomRepository;
    private readonly IRoomAssignmentRepository _assignmentRepository;
    private readonly IEventSender _eventSender;

    public InvoiceIssuedIntegrationEventHandler(
        CheckInWorkflow workflow,
        IRoomRepository roomRepository,
        IRoomAssignmentRepository assignmentRepository,
        IEventSender eventSender)
    {
        _workflow = workflow;
        _roomRepository = roomRepository;
        _assignmentRepository = assignmentRepository;
        _eventSender = eventSender;
    }

    public override string[] EventTypes => new[] { typeof(InvoiceIssuedIntegrationEvent).Name };

    protected override async Task<EventProcessingResult> OnHandleAsync(InvoiceIssuedIntegrationEvent eventData)
    {
        Console.WriteLine($"Received InvoiceIssuedIntegrationEvent: {eventData}");

        // Map integration event to domain command
        var command = new CheckInCommand(
            eventData.InvoiceId,
            eventData.BookingId,
            eventData.InvoiceNumber,
            eventData.CustomerName,
            eventData.CustomerEmail,
            eventData.RoomType,
            eventData.RoomNumber,
            eventData.CheckInDate,
            eventData.CheckOutDate,
            eventData.NetValue,
            eventData.VatValue,
            eventData.TotalValue,
            eventData.PaymentTransactionId,
            eventData.IssuedDate,
            eventData.Timestamp);

        // Execute the check-in workflow
        var result = await _workflow.ExecuteAsync(command, _roomRepository, _assignmentRepository);

        // Handle workflow result
        switch (result)
        {
            case CheckInCompletedEvent.CheckInSucceeded success:
                Console.WriteLine($"Check-in completed successfully: AssignmentId={success.AssignmentId}, Room={success.RoomNumber}, AccessCode={success.AccessCode}");
              
                
                return EventProcessingResult.Completed;

            case CheckInCompletedEvent.CheckInFailed failed:
                Console.WriteLine($"Check-in failed: {string.Join(", ", failed.Reasons)}");
                return EventProcessingResult.Failed;

            default:
                Console.WriteLine($"Unexpected workflow result: {result.GetType().Name}");
                return EventProcessingResult.Failed;
        }
    }

    
}
