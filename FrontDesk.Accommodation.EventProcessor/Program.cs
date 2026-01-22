using Example.Events.ServiceBus;
using FrontDesk.Data;
using FrontDesk.Data.Repositories;
using FrontDesk.Domain.Repositories;
using FrontDesk.Domain.Workflows;
using HotelBooking.Events;
using HotelBooking.Events.ServiceBus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FrontDesk.Accommodation.EventProcessor
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
                    services.AddDbContext<FrontDeskDbContext>(options => 
                        options.UseSqlServer(hostContext.Configuration.GetConnectionString("DefaultConnection")));
                    
                    // Repositories
                    services.AddScoped<IRoomRepository, RoomRepository>();
                    services.AddScoped<IRoomAssignmentRepository, RoomAssignmentRepository>();
                    
                    // Workflows
                    services.AddScoped<CheckInWorkflow>();
                    
                    // Azure Service Bus
                    services.AddAzureClients(builder =>
                    {
                        builder.AddServiceBusClient(hostContext.Configuration.GetConnectionString("ServiceBus"));
                    });

                    // Event sender
                    services.AddSingleton<IEventSender, ServiceBusTopicEventSender>();
                    
                    // Event listener and handler
                    services.AddSingleton<IEventListener, ServiceBusTopicEventListener>();
                    services.AddScoped<IEventHandler, InvoiceIssuedIntegrationEventHandler>();

                    // Background worker
                    services.AddHostedService<InvoiceEventWorker>();
                });
    }
}