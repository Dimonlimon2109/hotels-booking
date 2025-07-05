
using AutoMapper;
using HotelsBooking.BLL.DTO;
using HotelsBooking.DAL.Entities;

namespace HotelsBooking.BLL.Mappers
{
    public class CreateAmenityDtoToAmenityMapper : Profile
    {
        public CreateAmenityDtoToAmenityMapper()
        {
            CreateMap<CreateAmenityDTO, Amenity>();
        }
    }
}
