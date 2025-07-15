
using AutoMapper;
using HotelsBooking.BLL.DTO;
using HotelsBooking.DAL.Entities;

namespace HotelsBooking.BLL.Mappers
{
    public class ReviewToReviewDtoMapper : Profile
    {
        public ReviewToReviewDtoMapper()
        {
            CreateMap<Review, ReviewDTO>();
        }
    }
}
