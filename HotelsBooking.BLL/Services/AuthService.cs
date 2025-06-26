
using AutoMapper;
using FluentValidation;
using HotelsBooking.BLL.DTO;
using HotelsBooking.DAL.Entities;
using HotelsBooking.DAL.Interfaces;

namespace HotelsBooking.BLL.Services
{
   public class AuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<RegisterDTO> _registeringUserValidator;
        private readonly IValidator<LoginDTO> _loginingUserValidator;
        private readonly TokensService _tokensService;
        private readonly PasswordService _passwordService;

        public AuthService(
            IUserRepository userRepository,
            IMapper mapper,
            IValidator<RegisterDTO> registeringUserValidator,
            IValidator<LoginDTO> loginingUserValidator,
            TokensService tokensService,
            PasswordService passwordService
            )
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _registeringUserValidator = registeringUserValidator;
            _loginingUserValidator = loginingUserValidator;
            _tokensService = tokensService;
            _passwordService = passwordService;
        }

        public async Task RegisterAsync(RegisterDTO registeringUser, CancellationToken ct = default)
        {
            var validationResult = await _registeringUserValidator.ValidateAsync(registeringUser, ct);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var user = _mapper.Map<User>(registeringUser);
            await _userRepository.AddAsync(user);
        }

        public async Task<TokensDTO> LoginAsync(LoginDTO loginingInUser, CancellationToken ct = default)
        {
            var validationResult = await _loginingUserValidator.ValidateAsync(loginingInUser, ct);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var user = await _userRepository.GetByEmailAsync(loginingInUser.Email);
            if (user == null)
            {
                throw new NullReferenceException("Пользователь не найден");
            }

            if (!_passwordService.ValidatePassword(loginingInUser.Password, user.PasswordHash))
            {
                throw new InvalidOperationException("Неверный пароль");
            }

            var refreshToken = _tokensService.GenerateRefreshToken();
            user.RefreshToken = refreshToken.RefreshToken;
            user.RefreshTokenExpiresAt = refreshToken.ExpiresAt;
            await _userRepository.UpdateAsync(user);

            var accessToken = _tokensService.GenerateAccessToken(user);
            return new TokensDTO(accessToken, refreshToken.RefreshToken);
        }
    }
}
