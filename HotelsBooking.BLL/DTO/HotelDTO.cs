
namespace HotelsBooking.BLL.DTO
{
    public class HotelDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public int StarRating { get; set; }
        public string Description { get; set; }
        public IEnumerable<HotelPhotoDTO> Photos { get; set; }
    }
}
