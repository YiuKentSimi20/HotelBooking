using Booking.Dto.Events;
using Example.Events.Models;
using HotelBooking.Events;

namespace Booking.Accommodation.EventProcessor
{
  internal class BookingPaidIntegrationEventHandler : AbstractEventHandler<BookingPaidIntegrationEvent>
  {
    public override string[] EventTypes => new string[] { typeof(BookingPaidIntegrationEvent).Name };

    protected override Task<EventProcessingResult> OnHandleAsync(BookingPaidIntegrationEvent eventData)
    {
      Console.WriteLine("Data received from the ServiceBus paid-booking: \n" + eventData.ToString());
      return Task.FromResult(EventProcessingResult.Completed);
    }
  }
}
