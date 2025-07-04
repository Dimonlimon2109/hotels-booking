using AutoMapper;
using HotelsBooking.API.ViewModels;
using HotelsBooking.BLL.DTO;

namespace HotelsBooking.API.Mappers
{
    public class RoomDtoToRoomViewModelMapper:Profile
    {
        public RoomDtoToRoomViewModelMapper()
        {
            CreateMap<RoomDTO, RoomViewModel>();
        }
    }
}
