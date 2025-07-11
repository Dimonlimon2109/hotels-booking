
using HotelsBooking.DAL.Data;
using HotelsBooking.DAL.Entities;
using HotelsBooking.DAL.Interfaces;

namespace HotelsBooking.DAL.Repositories
{
    public class HotelPhotoRepository(ApplicationContext context) : Repository<HotelPhoto>(context), IHotelPhotoRepository
    {
    }
}
