
namespace HotelsBooking.DAL.Entities
{
    public class Hotel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public int StarRating { get; set; }
        public string Description { get; set; }
        public int OwnerId {  get; set; }
        public User Owner { get; set; }
        public ICollection<HotelPhoto> Photos { get; set; }
    }
}
