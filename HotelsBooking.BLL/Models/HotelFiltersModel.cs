
namespace HotelsBooking.BLL.Models
{
    public record HotelFiltersModel
        (
        string? Name,
        string? City,
        int? StarRating,
        IEnumerable<int> AmenityIds,
        int Limit, 
        int Offset,
        string? SortBy, //reviewRating, fromCenter
        string? Order = "asc"
        );
}
