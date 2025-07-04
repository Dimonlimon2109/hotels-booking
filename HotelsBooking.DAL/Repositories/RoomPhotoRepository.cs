
using HotelsBooking.DAL.Data;
using HotelsBooking.DAL.Entities;
using HotelsBooking.DAL.Interfaces;

namespace HotelsBooking.DAL.Repositories
{
    public class RoomPhotoRepository: Repository<RoomPhoto>, IRoomPhotoRepository
    {
        public RoomPhotoRepository(ApplicationContext context): base(context) { }
    }
}
