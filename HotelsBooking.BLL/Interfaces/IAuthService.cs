using HotelsBooking.BLL.DTO;

namespace HotelsBooking.BLL.Interfaces
{
    public interface IAuthService
    {
        Task<TokensDTO> LoginAsync(LoginDTO loginingInUser, CancellationToken ct = default);
        Task<TokensDTO> RefreshAsync(TokensDTO tokens, CancellationToken ct = default);
        Task RegisterAsync(RegisterDTO registeringUser, CancellationToken ct = default);
    }
}