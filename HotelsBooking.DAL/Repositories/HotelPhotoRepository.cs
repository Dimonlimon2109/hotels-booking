
using HotelsBooking.DAL.Data;
using HotelsBooking.DAL.Entities;
using HotelsBooking.DAL.Interfaces;

namespace HotelsBooking.DAL.Repositories
{
    public class HotelPhotoRepository : Repository<HotelPhoto>, IHotelPhotoRepository
    {
        public HotelPhotoRepository(ApplicationContext context) : base(context) { }

    }
}
