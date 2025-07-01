using AutoMapper;
using HotelsBooking.API.ViewModels;
using HotelsBooking.BLL.DTO;

namespace HotelsBooking.API.Mappers
{
    public class HotelPhotoDtoToHotelPhotoViewModelMapper:Profile
    {
        public HotelPhotoDtoToHotelPhotoViewModelMapper()
        {
            CreateMap<HotelPhotoDTO, HotelPhotoViewModel>();
        }
    }
}
