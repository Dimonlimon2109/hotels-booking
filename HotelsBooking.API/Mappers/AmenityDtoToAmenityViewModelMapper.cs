using AutoMapper;
using HotelsBooking.API.Models;
using HotelsBooking.API.ViewModels;
using HotelsBooking.BLL.DTO;

namespace HotelsBooking.API.Mappers
{
    public class AmenityDtoToAmenityViewModelMapper : Profile
    {
        public AmenityDtoToAmenityViewModelMapper()
        {
            CreateMap<AmenityDTO, AmenityViewModel>();
        }
    }
}
