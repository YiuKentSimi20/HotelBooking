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
      Console.WriteLine("Invoice Logging Worker started...");
      return eventListener.StartAsync("invoices", "invoices-logging-subscription", cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
      Console.WriteLine("Invoice Logging Worker stopped!");
      return eventListener.StopAsync(cancellationToken);
    }
  }
}