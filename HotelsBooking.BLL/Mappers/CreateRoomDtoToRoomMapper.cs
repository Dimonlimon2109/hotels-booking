
using AutoMapper;
using HotelsBooking.BLL.DTO;
using HotelsBooking.DAL.Constants;
using HotelsBooking.DAL.Entities;

namespace HotelsBooking.BLL.Mappers
{
    public class CreateRoomDtoToRoomMapper : Profile
    {
        public CreateRoomDtoToRoomMapper()
        {
            CreateMap<CreateRoomDTO, Room>()
                .ForMember(dest => dest.Type,
                    opt => opt.MapFrom(src =>
                        Enum.Parse<RoomType>(src.Type, true)));
        }
    }
}
