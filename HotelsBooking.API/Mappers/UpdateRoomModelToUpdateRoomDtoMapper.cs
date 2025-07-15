using AutoMapper;
using HotelsBooking.API.Models;
using HotelsBooking.BLL.DTO;

namespace HotelsBooking.API.Mappers
{
    public class UpdateRoomModelToUpdateRoomDtoMapper : Profile
    {
        public UpdateRoomModelToUpdateRoomDtoMapper()
        {
            CreateMap<UpdateRoomModel, UpdateRoomDTO>();
        }
    }
}
