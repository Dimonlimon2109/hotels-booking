using HotelsBooking.BLL.Models;
using HotelsBooking.DAL.Entities;
using System.Security.Claims;

namespace HotelsBooking.BLL.Interfaces
{
    public interface ITokensService
    {
        string GenerateAccessToken(User user);
        RefreshTokenModel GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromToken(string token);
    }
}