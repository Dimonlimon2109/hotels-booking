
using AutoMapper;
using FluentValidation;
using HotelsBooking.BLL.DTO;
using HotelsBooking.BLL.Interfaces;
using HotelsBooking.BLL.Models;
using HotelsBooking.BLL.Validators;
using HotelsBooking.DAL.Constants;
using HotelsBooking.DAL.Entities;
using HotelsBooking.DAL.Interfaces;
using HotelsBooking.DAL.Repositories;
using System.Security;

namespace HotelsBooking.BLL.Services
{
    public class RoomService : IRoomService
    {
        private readonly IUserRepository _userRepository;
        private readonly IHotelRepository _hotelRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IRoomPhotoRepository _roomPhotoRepository;
        private readonly IImageService _imageService;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateRoomDTO> _creatingRoomValidator;
        private readonly IValidator<UpdateRoomDTO> _updatingRoomValidator;

        public RoomService(
            IUserRepository userRepository,
            IHotelRepository hotelRepository,
            IRoomRepository roomRepository,
            IRoomPhotoRepository roomPhotoRepository,
            IImageService imageService,
            IMapper mapper,
            IValidator<CreateRoomDTO> creatingRoomValidator,
            IValidator<UpdateRoomDTO> updatingRoomValidator
            )
        {
            _userRepository = userRepository;
            _hotelRepository = hotelRepository;
            _roomRepository = roomRepository;
            _roomPhotoRepository = roomPhotoRepository;
            _imageService = imageService;
            _mapper = mapper;
            _creatingRoomValidator = creatingRoomValidator;
            _updatingRoomValidator = updatingRoomValidator;
        }
        public async Task<RoomDTO> CreateRoomAsync(string userEmail, CreateRoomDTO creatingRoom, CancellationToken ct = default)
        {
            var validationResult = _creatingRoomValidator.Validate(creatingRoom);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var user = await _userRepository.GetByEmailAsync(userEmail);
            var hotel = await _hotelRepository.GetByIdAsync(creatingRoom.HotelId);

            if (hotel == null)
            {
                throw new NullReferenceException("Отель не найден");
            }

            if (user == null || user.Id != hotel.OwnerId)
            {
                throw new SecurityException("Недостаточно прав на создание номера в этом отеле");
            }

            var room = _mapper.Map<Room>(creatingRoom);
            await _roomRepository.AddAsync(room, ct);
            await _roomRepository.SaveChangesAsync(ct);

            return _mapper.Map<RoomDTO>(room);
        }

        public async Task<IEnumerable<RoomDTO>> GetRoomsByHotelIdAsync(
            int hotelId,
            RoomFiltersModel filters,
            CancellationToken ct = default)
        {
            RoomType? type = null;

            if (!string.IsNullOrWhiteSpace(filters.Type))
            {
                if (Enum.TryParse<RoomType>(filters.Type, true, out var parsedType))
                {
                    type = parsedType;
                }
                else
                {
                    throw new ArgumentException("Неверный тип номера отеля.");
                }
            }
            var rooms = await _roomRepository.GetAllHotelsWithFiltersAsync(
                hotelId,
                filters.Limit,
                filters.Offset,
                type,
                filters.MinPrice,
                filters.MaxPrice,
                filters.Capacity,
                filters.SortBy,
                filters.Order,
                ct);
            return rooms.Select(r => _mapper.Map<RoomDTO>(r));
        }

        public async Task<int> GetTotalPagesAsync(int hotelId, RoomFiltersModel filters, CancellationToken ct = default)
        {
            RoomType? type = null;

            if (!string.IsNullOrWhiteSpace(filters.Type))
            {
                if (Enum.TryParse<RoomType>(filters.Type, true, out var parsedType))
                {
                    type = parsedType;
                }
                else
                {
                    throw new ArgumentException("Неверный тип номера отеля.");
                }
            }

            return await _roomRepository.GetRoomsTotalCountAsync(
                hotelId,
                type,
                filters.MinPrice,
                filters.MaxPrice,
                filters.Capacity,
                ct);
        }
        public async Task<RoomDTO> GetRoomByIdAsync(int roomId, CancellationToken ct = default)
        {
            var room = await _roomRepository.GetRoomAsync(roomId, ct)
                ?? throw new NullReferenceException("Номер в отеле не найден");
            return _mapper.Map<RoomDTO>(room);
        }

        public async Task UploadRoomPhotoAsync(int id, IImageFile image, string userEmail, CancellationToken ct = default)
        {
            var roomItem = await _roomRepository.GetByIdAsync(id, ct)
                ?? throw new NullReferenceException("Номер отеля не найден");

            var user = await _userRepository.GetByEmailAsync(userEmail, ct);
            var hotelItem = await _hotelRepository.GetByIdAsync(roomItem.HotelId, ct);
            if (hotelItem.OwnerId != user?.Id)
            {
                throw new SecurityException("Фотографию может добавить только владелец");
            }

            var imagePath = await _imageService.UploadImageAsync(image, "rooms", ct);
            var roomPhoto = new RoomPhoto
            {
                FilePath = imagePath,
                RoomId = roomItem.Id,
            };
            await _roomPhotoRepository.AddAsync(roomPhoto);
            await _roomRepository.SaveChangesAsync(ct);
        }

        public async Task DeleteRoomPhotoAsync(int roomId, int photoId, string userEmail, CancellationToken ct = default)
        {
            var roomItem = await _roomRepository.GetByIdAsync(roomId, ct)
                ?? throw new NullReferenceException("Номер в отеле не найден");

            var hotelItem = await _hotelRepository.GetByIdAsync(roomItem.HotelId, ct);
            var user = await _userRepository.GetByEmailAsync(userEmail, ct);
            if (hotelItem.OwnerId != user?.Id)
            {
                throw new SecurityException("Фотографии номера отеля может удалять только владелец");
            }
            var deletingPhoto = await _roomPhotoRepository.GetByIdAsync(photoId, ct)
                ?? throw new NullReferenceException("Фото номера отеля не найдено");

            await _imageService.DeleteImageAsync(deletingPhoto.FilePath);
            await _roomPhotoRepository.DeleteAsync(photoId, ct);
            await _roomRepository.SaveChangesAsync(ct);
        }

        public async Task DeleteRoomAsync(int id, string userEmail, CancellationToken ct = default)
        {
            var user = await _userRepository.GetByEmailAsync(userEmail, ct);
            var deletingRoom = await _roomRepository.GetByIdAsync(id, ct)
                ?? throw new NullReferenceException("Номер в отеле не найден");
            var hotel = await _hotelRepository.GetByIdAsync(deletingRoom.HotelId);

            if (user?.Id != hotel?.OwnerId)
            {
                throw new SecurityException("Номер отеля может удалить только владелец");
            }
            await _roomRepository.DeleteAsync(id, ct);
            await _roomRepository.SaveChangesAsync(ct);
        }

        public async Task UpdateRoomAsync(int roomId, string userEmail, UpdateRoomDTO updatingRoom, CancellationToken ct = default)
        {
            var roomItem = await _roomRepository.GetByIdAsync(roomId, ct)
                ?? throw new NullReferenceException("Номер отеля не найден");

            var validationResult = _updatingRoomValidator.Validate(updatingRoom);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var user = await _userRepository.GetByEmailAsync(userEmail, ct);
            var hotelItem = await _hotelRepository.GetByIdAsync(roomItem.HotelId, ct);
            if (hotelItem.OwnerId != user?.Id)
            {
                throw new SecurityException("Информацию о номере отеля может изменить только владелец");
            }

            roomItem.PricePerNight = updatingRoom.PricePerNight;
            roomItem.Capacity = updatingRoom.Capacity;
            roomItem.Type = Enum.Parse<RoomType>(updatingRoom.Type, true);
            _roomRepository.Update(roomItem);
            await _roomRepository.SaveChangesAsync(ct);
        }
    }
}
