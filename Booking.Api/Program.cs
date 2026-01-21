using Booking.Data;
using Booking.Data.Repositories;
using Booking.Domain.Repositories;
using Booking.Domain.Workflows;
using Example.Events.ServiceBus;
using HotelBooking.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using Microsoft.OpenApi.Models;

namespace Booking.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddDbContext<BookingDbContext>
                (options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddTransient<IBookingRepository, BookingRepository>();
            builder.Services.AddScoped<IRoomRepository, RoomRepository>();
            builder.Services.AddTransient<BookTicketWorkflow>();

            builder.Services.AddSingleton<IEventSender, ServiceBusTopicEventSender>();

            builder.Services.AddAzureClients(client =>
            {
                client.AddServiceBusClient(builder.Configuration.GetConnectionString("ServiceBus"));
            });

            builder.Services.AddHttpClient();

            builder.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Booking.Api", Version = "v1" });
            });


            WebApplication app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}