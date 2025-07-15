
using NetTopologySuite.Geometries;

namespace HotelsBooking.DAL.Entities
{
    public class Hotel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string HouseNumber { get; set; }
        public Point Location { get; set; }
        public int StarRating { get; set; }
        public double ReviewRating {  get; set; }
        public string Description { get; set; }
        public int OwnerId {  get; set; }
        public User Owner { get; set; }
        public ICollection<Amenity> Amenities { get; set; }
        public ICollection<HotelPhoto> Photos { get; set; }
        public ICollection<Room> Rooms { get; set; }
        public ICollection<Review> Reviews { get; set; }
    }
}
