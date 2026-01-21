using HotelBooking.Events;
using HotelBooking.Events.ServiceBus;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Booking.Accommodation.EventProcessor
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddAzureClients(builder =>
                    {
                        builder.AddServiceBusClient(hostContext.Configuration.GetConnectionString("ServiceBus"));
                    });

                    services.AddSingleton<IEventListener, ServiceBusTopicEventListener>();
                    services.AddSingleton<IEventHandler, BookingPaidIntegrationEventHandler>();

                    services.AddHostedService<EventLoggingWorker>();
                });
    }
}