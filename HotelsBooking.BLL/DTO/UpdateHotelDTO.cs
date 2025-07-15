
namespace HotelsBooking.BLL.DTO
{
    public class UpdateHotelDTO
    {
        public string Name { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string HouseNumber { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int StarRating { get; set; }
        public string Description { get; set; }
        public IEnumerable<int> AmenityIds { get; set; }
    }
}
