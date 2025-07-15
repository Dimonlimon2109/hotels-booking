namespace HotelsBooking.API.Models
{
    public record UpdateRoomModel
        (
        string Type,
        decimal PricePerNight,
        int Capacity
        );
}
