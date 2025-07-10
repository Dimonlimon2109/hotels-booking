namespace HotelsBooking.API.Models
{
    public record UpdateHotelModel
        (
        string Name,
        string Country,
        string City,
        string Street,
        string HouseNumber,
        decimal Latitude,
        decimal Longitude,
        double StarRating,
        string Description,
        IEnumerable<int> AmenityIds
        );
}
