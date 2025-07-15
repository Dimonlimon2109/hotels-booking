using AutoMapper;
using HotelsBooking.API.ViewModels;
using HotelsBooking.BLL.DTO;

namespace HotelsBooking.API.Mappers
{
    public class HotelDtoToHotelViewModelMapper: Profile
    {
        public HotelDtoToHotelViewModelMapper()
        {
            CreateMap<HotelDTO, HotelViewModel>();
        }
    }
}
