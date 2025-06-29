
using AutoMapper;
using FluentValidation;
using HotelsBooking.BLL.DTO;
using HotelsBooking.DAL.Entities;
using HotelsBooking.DAL.Interfaces;
using Microsoft.IdentityModel.Tokens.Experimental;
using System.Security;

namespace HotelsBooking.BLL.Services
{
    public class HotelService
    {
        private readonly IHotelRepository _hotelRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateHotelDTO> _creatingHotelValidator;
        private readonly IValidator<UpdateHotelDTO> _updatingHotelValidator;


        public HotelService(
            IHotelRepository hotelRepository,
            IUserRepository userRepository,
            IMapper mapper,
            IValidator<CreateHotelDTO> creatingHotelValidator,
            IValidator<UpdateHotelDTO> updatingHotelValidator
            )
        {
            _hotelRepository = hotelRepository;
            _userRepository = userRepository;
            _mapper = mapper;
            _creatingHotelValidator = creatingHotelValidator;
            _updatingHotelValidator = updatingHotelValidator;
        }

        public async Task<HotelDTO> CreateHotelAsync(string userEmail, CreateHotelDTO creatingHotel, CancellationToken ct = default)
        {
            var validationResult = _creatingHotelValidator.Validate(creatingHotel);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var user = await _userRepository.GetByEmailAsync(userEmail);
            if (user == null)
            {
                throw new SecurityException("Пользователь не найден");
            }

            var hotel = _mapper.Map<Hotel>(creatingHotel);
            hotel.OwnerId = user.Id;
            await _hotelRepository.AddAsync(hotel, ct);
            return _mapper.Map<HotelDTO>(hotel);
        }

        public async Task<IEnumerable<HotelDTO>> GetAllHotelsAsync(CancellationToken ct = default)
        {
            var hotels = await _hotelRepository.GetAllAsync(ct);
            return hotels.Select(h => _mapper.Map<HotelDTO>(h));
        }

        public async Task<HotelDTO> GetHotelAsync(int id, CancellationToken ct = default)
        {
            var hotel = await _hotelRepository.GetByIdAsync(id, ct);
            return _mapper.Map<HotelDTO>(hotel);
        }

        public async Task<IEnumerable<HotelDTO>> GetMyHotelsAsync(string userEmail, CancellationToken ct = default)
        {
            var user = await _userRepository.GetByEmailAsync(userEmail, ct)
                ?? throw new SecurityException("Пользователь не найден");
            var userHotels = await _hotelRepository.GetOwnerHotelsAsync(user.Id, ct);
            return userHotels?.Select(h => _mapper.Map<HotelDTO>(h)) ?? new List<HotelDTO>();
        }

        public async Task DeleteHotelAsync(int id, string userEmail, CancellationToken ct = default)
        {
            var user = await _userRepository.GetByEmailAsync(userEmail, ct);
            var deletingHotel = await _hotelRepository.GetHotelByIdWithOwnerAsync(id, ct);
            if(user?.Id != deletingHotel?.OwnerId)
            {
                throw new SecurityException("Отель может удалить только владелец");
            }
            await _hotelRepository.DeleteAsync(id, ct);
        }

        public async Task UpdateHotelAsync(int id, string userEmail, UpdateHotelDTO updatingHotel,  CancellationToken ct = default)
        {
            var hotelItem = await _hotelRepository.GetHotelByIdWithOwnerAsync(id, ct)
                ?? throw new NullReferenceException("Отель не найден");

            var validationResult = _updatingHotelValidator.Validate(updatingHotel);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var user = await _userRepository.GetByEmailAsync(userEmail, ct);
            if(hotelItem.OwnerId != user?.Id)
            {
                throw new SecurityException("Информацию об отеле может изменить только владелец");
            }

            hotelItem.Name = updatingHotel.Name;
            hotelItem.Address = updatingHotel.Address;
            hotelItem.Latitude = updatingHotel.Latitude;
            hotelItem.Longitude = updatingHotel.Longitude;
            hotelItem.Description = updatingHotel.Description;
            await _hotelRepository.UpdateAsync(hotelItem);
        }
    }
}
