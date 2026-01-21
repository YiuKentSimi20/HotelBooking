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
      Console.WriteLine("Worker started...");
      return eventListener.StartAsync("bookings", "paid-bookings", cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
      Console.WriteLine("Worker stopped!");
      return eventListener.StopAsync(cancellationToken);
    }
  }
}