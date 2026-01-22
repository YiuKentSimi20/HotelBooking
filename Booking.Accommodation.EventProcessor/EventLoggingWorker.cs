using HotelBooking.Events;
using Microsoft.Extensions.Hosting;

namespace Booking.Accommodation.EventProcessor
{
  internal class EventLoggingWorker : IHostedService
  {
    private readonly IEventListener eventListener;

    public EventLoggingWorker(IEventListener eventListener)
    {
      this.eventListener = eventListener;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
      Console.WriteLine("Booking logging Worker started...");
      return eventListener.StartAsync("bookings", "bookings-logging-subscription", cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
      Console.WriteLine("Booking logging Worker stopped!");
      return eventListener.StopAsync(cancellationToken);
    }
  }
}