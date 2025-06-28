
using AutoMapper;
using HotelsBooking.BLL.DTO;
using HotelsBooking.DAL.Entities;

namespace HotelsBooking.BLL.Mappers
{
    public class HotelToHotelDtoMapper : Profile
    {
        public HotelToHotelDtoMapper()
        {
            CreateMap<Hotel, HotelDTO>();
        }
    }
}
