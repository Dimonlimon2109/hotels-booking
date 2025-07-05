using AutoMapper;
using HotelsBooking.API.Models;
using HotelsBooking.BLL.DTO;

namespace HotelsBooking.API.Mappers
{
    public class CreateReviewModelToCreateReviewDtoMapper : Profile
    {
        public CreateReviewModelToCreateReviewDtoMapper()
        {
            CreateMap<CreateReviewModel, CreateReviewDTO>();
        }
    }
}
