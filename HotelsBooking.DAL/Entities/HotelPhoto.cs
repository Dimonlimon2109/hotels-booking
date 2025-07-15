
namespace HotelsBooking.DAL.Entities
{
    public class HotelPhoto:PhotoBase
    {
        public int HotelId { get; set; }
        public Hotel Hotel { get; set; }
    }
}
