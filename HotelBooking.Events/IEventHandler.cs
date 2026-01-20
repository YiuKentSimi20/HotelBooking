using CloudNative.CloudEvents;
using Example.Events.Models;

namespace HotelBooking.Events
{
    public interface IEventHandler
    {
        string[] EventTypes { get; }

        Task<EventProcessingResult> HandleAsync(CloudEvent cloudEvent);
    }
}
