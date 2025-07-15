
namespace HotelsBooking.BLL.DTO
{
    public class CreateRoomDTO
    {
        public int HotelId { get; set; }
        public string Type { get; set; }
        public decimal PricePerNight { get; set; }
        public int Capacity { get; set; }
    }
}
