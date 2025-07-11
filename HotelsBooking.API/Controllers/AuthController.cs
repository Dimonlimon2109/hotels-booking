using AutoMapper;
using HotelsBooking.API.Constants;
using HotelsBooking.API.Models;
using HotelsBooking.BLL.DTO;
using HotelsBooking.BLL.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HotelsBooking.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        private readonly IMapper _mapper;

        public AuthController(AuthService authService, IMapper mapper)
        {
            _authService = authService;
            _mapper = mapper;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterModel registeringUser, CancellationToken ct)
        {
            var registerDTO = _mapper.Map<RegisterDTO>(registeringUser);
            await _authService.RegisterAsync(registerDTO);
            return Ok(HttpMessages.SuccessRegistering);
        }

        [HttpPost("login")]

        public async Task<IActionResult> Login(LoginModel loginingInUser, CancellationToken ct)
        {
            var loginDTO = _mapper.Map<LoginDTO>(loginingInUser);
            var tokens = await _authService.LoginAsync(loginDTO);
            return Ok(tokens);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh(TokensModel tokens, CancellationToken ct)
        {
            var tokensDTO = _mapper.Map<TokensDTO>(tokens);
            var responseTokens = await _authService.RefreshAsync(tokensDTO);
            return Ok(responseTokens);
        }
    }
}
