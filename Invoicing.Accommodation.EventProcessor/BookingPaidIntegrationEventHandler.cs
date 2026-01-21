using Example.Events.Models;
using HotelBooking.Events;
using Invoicing.Dto.Events;

namespace Invoicing.Accommodation.EventProcessor
{
  internal class BookingPaidIntegrationEventHandler : AbstractEventHandler<BookingPaidIntegrationEvent>
  {
    public override string[] EventTypes => new string[] { typeof(BookingPaidIntegrationEvent).Name };

    protected override Task<EventProcessingResult> OnHandleAsync(BookingPaidIntegrationEvent eventData)
    {
      // aici ar trebui sa fie inceapa workflow-ul de facturare
      Console.WriteLine(eventData.ToString());
      return Task.FromResult(EventProcessingResult.Completed);
    }
  }
}
