using AutoMapper;
using HotelsBooking.API.ViewModels;
using HotelsBooking.BLL.DTO;

namespace HotelsBooking.API.Mappers
{
    public class PhotoDtoToPhotoViewModelMapper:Profile
    {
        public PhotoDtoToPhotoViewModelMapper()
        {
            CreateMap<PhotoDTO, PhotoViewModel>();
        }
    }
}
