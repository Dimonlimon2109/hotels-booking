namespace HotelsBooking.API.Models
{
    public record UpdateHotelModel
        (
        string Name,
        string Country,
        string City,
        string Street,
        string HouseNumber,
        double Latitude,
        double Longitude,
        double StarRating,
        string Description,
        IEnumerable<int> AmenityIds
        );
}
