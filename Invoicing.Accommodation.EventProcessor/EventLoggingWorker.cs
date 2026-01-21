using HotelBooking.Events;
using Microsoft.Extensions.Hosting;

namespace Invoicing.Accommodation.EventProcessor
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
      Console.WriteLine("Invoice Worker started...");
      return eventListener.StartAsync("bookings", "paid_bookings", cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
      Console.WriteLine("Invoice Worker stopped!");
      return eventListener.StopAsync(cancellationToken);
    }
  }
}