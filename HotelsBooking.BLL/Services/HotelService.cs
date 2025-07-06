
using AutoMapper;
using FluentValidation;
using HotelsBooking.BLL.DTO;
using HotelsBooking.BLL.Interfaces;
using HotelsBooking.DAL.Entities;
using HotelsBooking.DAL.Interfaces;
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
        private readonly ImageService _imageService;
        private readonly IHotelPhotoRepository _hotelPhotoRepository;
        private readonly IAmenityRepository _amenityRepository;


        public HotelService(
            IHotelRepository hotelRepository,
            IUserRepository userRepository,
            IMapper mapper,
            IValidator<CreateHotelDTO> creatingHotelValidator,
            IValidator<UpdateHotelDTO> updatingHotelValidator,
            ImageService imageService,
            IHotelPhotoRepository hotelPhotoRepository,
            IAmenityRepository amenityRepository
            )
        {
            _hotelRepository = hotelRepository;
            _userRepository = userRepository;
            _mapper = mapper;
            _creatingHotelValidator = creatingHotelValidator;
            _updatingHotelValidator = updatingHotelValidator;
            _imageService = imageService;
            _hotelPhotoRepository = hotelPhotoRepository;
            _amenityRepository = amenityRepository;
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

            var amenities = await _amenityRepository.GetExistingAmenitiesAsync(creatingHotel.AmenityIds);
            var notFoundIds = creatingHotel.AmenityIds.Except(amenities.Select(a => a.Id));
            if (notFoundIds.Any())
            {
                throw new ArgumentException($"Некорректные AmenityIds: {string.Join(", ", notFoundIds)}");
            }

            var hotel = _mapper.Map<Hotel>(creatingHotel);
            hotel.Amenities = amenities.ToList();
            hotel.OwnerId = user.Id;
            await _hotelRepository.AddAsync(hotel, ct);
            return _mapper.Map<HotelDTO>(hotel);
        }

        public async Task<IEnumerable<HotelDTO>> GetAllHotelsAsync(CancellationToken ct = default)
        {
            var hotels = await _hotelRepository.GetHotelsAsync(ct);
            var hotelsDTO = hotels.Select(h => _mapper.Map<HotelDTO>(h));
            return hotelsDTO;
        }

        public async Task<HotelDTO> GetHotelAsync(int id, CancellationToken ct = default)
        {
            var hotel = await _hotelRepository.GetHotelAsync(id, ct);
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

            var amenities = await _amenityRepository.GetExistingAmenitiesAsync(updatingHotel.AmenityIds);
            var notFoundIds = updatingHotel.AmenityIds.Except(amenities.Select(a => a.Id));
            if (notFoundIds.Any())
            {
                throw new ArgumentException($"Некорректные AmenityIds: {string.Join(", ", notFoundIds)}");
            }

            hotelItem.Name = updatingHotel.Name;
            hotelItem.Address = updatingHotel.Address;
            hotelItem.Latitude = updatingHotel.Latitude;
            hotelItem.Longitude = updatingHotel.Longitude;
            hotelItem.Description = updatingHotel.Description;
            hotelItem.StarRating = updatingHotel.StarRating;
            hotelItem.Amenities = amenities.ToList();
            await _hotelRepository.UpdateAsync(hotelItem);
        }

        public async Task UploadHotelPhotoAsync(int id, IImageFile image, string userEmail, CancellationToken ct = default)
        {
            var hotelItem = await _hotelRepository.GetByIdAsync(id, ct)
                ?? throw new NullReferenceException("Отель не найден");

            var user = await _userRepository.GetByEmailAsync(userEmail, ct);
            if (hotelItem.OwnerId != user?.Id)
            {
                throw new SecurityException("Фотографию может добавить только владелец");
            }

            var imagePath = await _imageService.UploadImageAsync(image, "hotels", ct);
            var hotelPhoto = new HotelPhoto
            {
                FilePath = imagePath,
                HotelId = hotelItem.Id,
            };
            await _hotelPhotoRepository.AddAsync(hotelPhoto);
        }

        public async Task DeleteHotelPhotoAsync(int hotelId, int photoId, string userEmail, CancellationToken ct = default)
        {
            var hotelItem = await _hotelRepository.GetByIdAsync(hotelId, ct)
                ?? throw new NullReferenceException("Отель не найден");

            var user = await _userRepository.GetByEmailAsync(userEmail, ct);
            if (hotelItem.OwnerId != user?.Id)
            {
                throw new SecurityException("Фотографии отеля может удалять только владелец");
            }
            var deletingPhoto = await _hotelPhotoRepository.GetByIdAsync(photoId, ct)
                ?? throw new NullReferenceException("Фото отеля не найдено");

            await _imageService.DeleteImageAsync(deletingPhoto.FilePath);
            await _hotelPhotoRepository.DeleteAsync(photoId, ct);
        }
    }
}
