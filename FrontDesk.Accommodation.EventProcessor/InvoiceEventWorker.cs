using HotelBooking.Events;
using Microsoft.Extensions.Hosting;

namespace FrontDesk.Accommodation.EventProcessor
{
  internal class InvoiceEventWorker : IHostedService
  {
    private readonly IEventListener eventListener;

    public InvoiceEventWorker(IEventListener eventListener)
    {
      this.eventListener = eventListener;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
      Console.WriteLine("Invoices listener Worker started...");
      return eventListener.StartAsync("invoices", "invoices-subscription", cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
      Console.WriteLine("Invoices listener Worker stopped!");
      return eventListener.StopAsync(cancellationToken);
    }
  }
}