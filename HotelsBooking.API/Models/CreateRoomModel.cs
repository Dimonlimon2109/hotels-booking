namespace HotelsBooking.API.Models
{
    public record CreateRoomModel(
        int HotelId,
        string Type, 
        decimal PricePerNight,
        int Capacity
    );
}
