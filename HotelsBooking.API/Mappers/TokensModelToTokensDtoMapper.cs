using AutoMapper;
using HotelsBooking.API.Models;
using HotelsBooking.BLL.DTO;

namespace HotelsBooking.API.Mappers
{
    public class TokensModelToTokensDtoMapper: Profile
    {
        public TokensModelToTokensDtoMapper()
        {
            CreateMap<TokensModel, TokensDTO>();
        }
    }
}
