using Example.Events.Models;
using HotelBooking.Events;
using Invoicing.Domain.Models.Commands;
using Invoicing.Domain.Models.Events;
using Invoicing.Domain.Repositories;
using Invoicing.Domain.Workflows;
using Invoicing.Dto.Events;

namespace Invoicing.Accommodation.EventProcessor;

internal class BookingPaidIntegrationEventHandler : AbstractEventHandler<BookingPaidIntegrationEvent>
{
    private readonly GenerateInvoiceWorkflow _workflow;
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly IEventSender _eventSender;

    public BookingPaidIntegrationEventHandler(
        GenerateInvoiceWorkflow workflow,
        IInvoiceRepository invoiceRepository,
        IEventSender eventSender)
    {
        _workflow = workflow;
        _invoiceRepository = invoiceRepository;
        _eventSender = eventSender;
    }

    public override string[] EventTypes => new[] { typeof(BookingPaidIntegrationEvent).Name };

    protected override async Task<EventProcessingResult> OnHandleAsync(BookingPaidIntegrationEvent eventData)
    {
        Console.WriteLine($"Received BookingPaidIntegrationEvent: {eventData}");

        // Map integration event to domain command
        var command = new GenerateInvoiceCommand(
            eventData.BookingId,
            eventData.CustomerName,
            eventData.CustomerEmail,
            eventData.RoomType,
            eventData.RoomNumber,
            eventData.CheckInDate,
            eventData.CheckOutDate,
            eventData.TotalAmount,
            eventData.PaymentTransactionId,
            eventData.Timestamp);

        // Execute the invoice generation workflow
        var result = await _workflow.ExecuteAsync(command, _invoiceRepository);

        // Handle workflow result
        switch (result)
        {
            case InvoiceIssuedEvent.InvoiceSucceeded success:
                Console.WriteLine($"Invoice generated successfully: InvoiceId={success.InvoiceId}, InvoiceNumber={success.InvoiceNumber}, Total={success.TotalValue:C}");
                
                // Send integration event to "invoices" topic
                await PublishInvoiceIssuedEventAsync(success);
                
                return EventProcessingResult.Completed;

            case InvoiceIssuedEvent.InvoiceFailed failed:
                Console.WriteLine($"Invoice generation failed: {string.Join(", ", failed.Reasons)}");
                return EventProcessingResult.Failed;

            default:
                Console.WriteLine($"Unexpected workflow result: {result.GetType().Name}");
                return EventProcessingResult.Failed;
        }
    }

    private async Task PublishInvoiceIssuedEventAsync(InvoiceIssuedEvent.InvoiceSucceeded success)
    {
        var integrationEvent = new InvoiceIssuedIntegrationEvent
        {
            InvoiceId = success.InvoiceId,
            BookingId = success.BookingId,
            InvoiceNumber = success.InvoiceNumber,
            CustomerName = success.CustomerName,
            CustomerEmail = success.CustomerEmail,
            RoomType = success.RoomType,
            RoomNumber = success.RoomNumber,
            CheckInDate = success.CheckInDate,
            CheckOutDate = success.CheckOutDate,
            NetValue = success.NetValue,
            VatValue = success.VatValue,
            TotalValue = success.TotalValue,
            PaymentTransactionId = success.PaymentTransactionId,
            IssuedDate = success.IssuedDate,
            Timestamp = DateTime.UtcNow
        };

        await _eventSender.SendAsync("invoices", integrationEvent);
        Console.WriteLine($"Published InvoiceIssuedIntegrationEvent to 'invoices' topic: {integrationEvent}");
    }
}
