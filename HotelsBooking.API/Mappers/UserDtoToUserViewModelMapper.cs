using AutoMapper;
using HotelsBooking.API.ViewModels;
using HotelsBooking.BLL.DTO;

namespace HotelsBooking.API.Mappers
{
    public class UserDtoToUserViewModelMapper : Profile
    {
        public UserDtoToUserViewModelMapper()
        {
            CreateMap<UserDTO, UserViewModel>();
        }
    }
}
