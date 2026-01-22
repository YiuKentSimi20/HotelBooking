using Example.Events.Models;
using HotelBooking.Events;
using Invoicing.Dto.Events;

namespace Invoicing.Accommodation.EventProcessor
{
    internal class InvoiceIssuedIntegrationEventHandler : AbstractEventHandler<InvoiceIssuedIntegrationEvent>
    {
        public override string[] EventTypes => new string[] { typeof(InvoiceIssuedIntegrationEvent).Name };

        protected override Task<EventProcessingResult> OnHandleAsync(InvoiceIssuedIntegrationEvent eventData)
        {
            Console.WriteLine("Data received from the ServiceBus invoices-subscription: \n" + eventData.ToString());
            return Task.FromResult(EventProcessingResult.Completed);
        }
    }
}