
using AutoMapper;
using HotelsBooking.BLL.DTO;
using HotelsBooking.DAL.Entities;

namespace HotelsBooking.BLL.Mappers
{
    public class CreateHotelDtoToHotelMapper : Profile
    {
        public CreateHotelDtoToHotelMapper()
        {
            CreateMap<CreateHotelDTO, Hotel>()
                .ForMember(dest => dest.ReviewRating, opt => opt.MapFrom(_ => 0));
        }
    }
}
