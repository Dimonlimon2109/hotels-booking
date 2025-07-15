
namespace HotelsBooking.DAL.Entities
{
    public class RoomPhoto : PhotoBase
    {
        public int RoomId {  get; set; }
        public Room Room { get; set; }
    }
}
