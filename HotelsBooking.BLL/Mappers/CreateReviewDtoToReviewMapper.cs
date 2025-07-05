
using AutoMapper;
using HotelsBooking.BLL.DTO;
using HotelsBooking.DAL.Entities;

namespace HotelsBooking.BLL.Mappers
{
    public class CreateReviewDtoToReviewMapper : Profile
    {
        public CreateReviewDtoToReviewMapper()
        {
            CreateMap<CreateReviewDTO, Review>();
        }
    }
}
