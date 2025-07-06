namespace HotelsBooking.API.Models
{
    public record UpdateHotelModel
        (
        string Name,
        string Address,
        decimal Latitude,
        decimal Longitude,
        double StarRating,
        string Description,
        IEnumerable<int> AmenityIds
        );
}
