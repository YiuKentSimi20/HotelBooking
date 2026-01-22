using Microsoft.AspNetCore.Mvc;
using Booking.Api.Models;
using Booking.Domain.Models.Commands;
using Booking.Domain.Models.Events;
using Booking.Domain.Workflows;
using Booking.Domain.Repositories;
using HotelBooking.Events;
using Booking.Dto.Events;



namespace Booking.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookingController : ControllerBase
{
    private readonly BookTicketWorkflow _workflow;
    private readonly IEventSender _eventSender;
    private readonly IRoomRepository _roomRepository;
    private readonly IBookingRepository _bookingRepository;

    public BookingController(
        BookTicketWorkflow workflow, 
        IEventSender eventSender, 
        IRoomRepository roomRepository,
        IBookingRepository bookingRepository)
    {
        _workflow = workflow;
        _eventSender = eventSender;
        _roomRepository = roomRepository;
        _bookingRepository = bookingRepository;
    }

    [HttpGet("rooms")]
    public async Task<IActionResult> GetAllRooms()
    {
        var roomsWithAssignments = await _roomRepository.GetAllRoomsWithAssignmentsAsync();

        var response = roomsWithAssignments.Select(room => new RoomAvailabilityResponse
        {
            RoomId = room.RoomId,
            RoomNumber = room.RoomNumber,
            RoomType = room.RoomType,
            IsClean = room.IsClean,
            IsOutOfService = room.IsOutOfService,
            PricePerNight = room.PricePerNight,
            BookedPeriods = room.Assignments.Select(a => new BookingPeriodInfo
            {
                AssignmentId = a.AssignmentId,
                BookingId = a.BookingId,
                CheckInDate = a.CheckInDate.ToString("yyyy-MM-dd"),
                CheckOutDate = a.CheckOutDate?.ToString("yyyy-MM-dd")
            }).ToList()
        }).ToList();

        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> CreateBooking([FromBody] InputBooking input)
    {
        var command = MapInputToCommand(input);
        
        var workflowResult = await _workflow.ExecuteAsync(
            command,
            ProcessPaymentMock,
            _bookingRepository,
            _roomRepository);

        IActionResult response = workflowResult switch
        {
            BookingPaidEvent.BookingSucceeded succeeded => await PublishBookingSucceededEvent(succeeded),
            BookingPaidEvent.BookingFailed failed => BadRequest(new { success = false, errors = failed.Reasons }),
            _ => StatusCode(500, new { success = false, error = "Unexpected workflow result" })
        };

        return response;
    }

    private async Task<IActionResult> PublishBookingSucceededEvent(BookingPaidEvent.BookingSucceeded successEvent)
    {
        await _eventSender.SendAsync("bookings", new BookingPaidIntegrationEvent
        {
            BookingId = successEvent.BookingId,
            CustomerName = successEvent.CustomerName,
            CustomerEmail = successEvent.CustomerEmail,
            RoomType = successEvent.RoomType,
            RoomNumber = successEvent.RoomNumber,
            CheckInDate = successEvent.CheckInDate.ToDateTime(TimeOnly.MinValue),
            CheckOutDate = successEvent.CheckOutDate.ToDateTime(TimeOnly.MinValue),
            TotalAmount = successEvent.TotalAmount,
            PaymentTransactionId = successEvent.PaymentTransactionId,
            Timestamp = DateTime.UtcNow
        });

        return Ok(new
        {
            success = true, 
            bookingId = successEvent.BookingId, RoomNumber = successEvent.RoomNumber,
            checkInDate = successEvent.CheckInDate.ToString("yyyy-MM-dd"), 
            checkOutDate = successEvent.CheckOutDate.ToString("yyyy-MM-dd")
        });
    }

    private static BookTicketCommand MapInputToCommand(InputBooking input) => new(
        CustomerName: input.CustomerName,
        CustomerEmail: input.CustomerEmail,
        RoomType: input.RoomType,
        CheckInDate: input.CheckInDate.ToString("yyyy-MM-dd"),
        CheckOutDate: input.CheckOutDate.ToString("yyyy-MM-dd")
    );

    private static string ProcessPaymentMock(string customerEmail, decimal totalAmount)
    {
        // Mock payment processing - in production, integrate with real payment gateway
        return $"TXN-{Guid.NewGuid().ToString()[..8].ToUpper()}";
    }
}
