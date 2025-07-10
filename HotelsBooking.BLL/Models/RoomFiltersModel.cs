
namespace HotelsBooking.BLL.Models
{
    public record RoomFiltersModel(
        string? Type,
        decimal? MinPrice,
        decimal? MaxPrice,
        int? Capacity,
        int Limit,
        int Offset,
        string? SortBy,
        string? Order = "asc"
        );
}
