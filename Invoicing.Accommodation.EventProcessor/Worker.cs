using HotelBooking.Events;
using Microsoft.Extensions.Hosting;

namespace Invoicing.Accommodation.EventProcessor
{
  internal class Worker : IHostedService
  {
    private readonly IEventListener eventListener;

    public Worker(IEventListener eventListener)
    {
      this.eventListener = eventListener;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
      Console.WriteLine("Worker started...");
      return eventListener.StartAsync("grades", "acctest", cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
      Console.WriteLine("Worker stopped!");
      return eventListener.StopAsync(cancellationToken);
    }
  }
}