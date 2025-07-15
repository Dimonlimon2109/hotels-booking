
using AutoMapper;
using HotelsBooking.BLL.DTO;
using HotelsBooking.DAL.Entities;

namespace HotelsBooking.BLL.Mappers
{
    public class HotelToHotelDtoMapper : Profile
    {
        public HotelToHotelDtoMapper()
        {
            CreateMap<Hotel, HotelDTO>()
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src =>
                string.Join (", ", new[]
                {
                    src.Country,
                    src.City,
                    src.Street,
                    $"д. {src.HouseNumber}"
                })));
        }
    }
}
