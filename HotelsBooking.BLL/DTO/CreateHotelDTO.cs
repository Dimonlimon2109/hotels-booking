
namespace HotelsBooking.BLL.DTO
{
    public class CreateHotelDTO
    {
        public string Name { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string HouseNumber { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public int StarRating { get; set; }
        public string Description { get; set; }
        public IEnumerable<int> AmenityIds { get; set; }
    }
}
