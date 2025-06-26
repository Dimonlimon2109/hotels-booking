
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
        private readonly TokensService _tokensService;
        private readonly PasswordService _passwordService;

        public AuthService(
            IUserRepository userRepository,
            IMapper mapper,
            IValidator<RegisterDTO> registeringUserValidator,
            TokensService tokensService,
            PasswordService passwordService
            )
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _registeringUserValidator = registeringUserValidator;
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
    }
}
