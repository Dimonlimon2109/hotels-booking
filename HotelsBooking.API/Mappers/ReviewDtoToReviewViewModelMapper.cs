using AutoMapper;
using HotelsBooking.API.ViewModels;
using HotelsBooking.BLL.DTO;

namespace HotelsBooking.API.Mappers
{
    public class ReviewDtoToReviewViewModelMapper : Profile
    {
        public ReviewDtoToReviewViewModelMapper()
        {
            CreateMap<ReviewDTO, ReviewViewModel>();
        }
    }
}
