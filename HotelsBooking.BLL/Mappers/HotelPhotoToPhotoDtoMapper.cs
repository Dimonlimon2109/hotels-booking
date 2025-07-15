using AutoMapper;
using HotelsBooking.BLL.DTO;
using HotelsBooking.DAL.Entities;

namespace HotelsBooking.BLL.Mappers
{
    public class HotelPhotoToPhotoDtoMapper : Profile
    {
        public HotelPhotoToPhotoDtoMapper()
        {
            CreateMap<HotelPhoto, PhotoDTO>();
        }
    }
}
