namespace HotelsBooking.API.Models
{
    public record UpdateHotelModel
        (
        string Name,
        string Address,
        decimal Latitude,
        decimal Longitude,
        string Description
        );
}
