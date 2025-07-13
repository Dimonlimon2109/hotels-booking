
using HotelsBooking.DAL.Data;
using HotelsBooking.DAL.Entities;
using HotelsBooking.DAL.Interfaces;

namespace HotelsBooking.DAL.Repositories
{
    public class RoomPhotoRepository(ApplicationContext context) : Repository<RoomPhoto>(context), IRoomPhotoRepository
    {
    }
}
