namespace HotelsBooking.API.Models
{
    public record CreateHotelModel
        (
        string Name,
        string Address,
        decimal Latitude,
        decimal Longitude,
        string Description
        );
}
