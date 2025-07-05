
using AutoMapper;
using HotelsBooking.BLL.DTO;
using HotelsBooking.DAL.Entities;

namespace HotelsBooking.BLL.Mappers
{
    public class AmenityToAmenityDtoMapper : Profile
    {
        public AmenityToAmenityDtoMapper()
        {
            CreateMap<Amenity, AmenityDTO>();
        }
    }
}
