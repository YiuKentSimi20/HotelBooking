using Example.Events.ServiceBus;
using HotelBooking.Events;
using HotelBooking.Events.ServiceBus;
using Invoicing.Data;
using Invoicing.Data.Repositories;
using Invoicing.Domain.Repositories;
using Invoicing.Domain.Workflows;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Invoicing.Accommodation.EventProcessor
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
                    // Database
                    services.AddDbContext<InvoicingDbContext>(options => 
                        options.UseSqlServer(hostContext.Configuration.GetConnectionString("DefaultConnection")));
                    
                    // Repositories
                    services.AddScoped<IInvoiceRepository, InvoiceRepository>();
                    
                    // Workflows
                    services.AddScoped<GenerateInvoiceWorkflow>();
                    
                    // Azure Service Bus
                    services.AddAzureClients(builder =>
                    {
                        builder.AddServiceBusClient(hostContext.Configuration.GetConnectionString("ServiceBus"));
                    });

                    // Event sender
                    services.AddSingleton<IEventSender, ServiceBusTopicEventSender>();
                    
                    // Event listener and handler
                    services.AddSingleton<IEventListener, ServiceBusTopicEventListener>();
                    services.AddScoped<IEventHandler, BookingPaidIntegrationEventHandler>();
                    services.AddScoped<IEventHandler, InvoiceIssuedIntegrationEventHandler>();

                    // Background worker
                    services.AddHostedService<BookingEventWorker>();
                    services.AddHostedService<EventLoggingWorker>();
                });
    }
}