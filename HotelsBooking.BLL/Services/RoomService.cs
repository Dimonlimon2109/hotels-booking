
using AutoMapper;
using FluentValidation;
using HotelsBooking.BLL.DTO;
using HotelsBooking.BLL.Validators;
using HotelsBooking.DAL.Entities;
using HotelsBooking.DAL.Interfaces;
using System.Security;

namespace HotelsBooking.BLL.Services
{
    public class RoomService
    {
        private readonly IUserRepository _userRepository;
        private readonly IHotelRepository _hotelRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IRoomPhotoRepository _roomPhotoRepository;
        private readonly ImageService _imageService;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateRoomDTO> _creatingRoomValidator;

        public RoomService(
            IUserRepository userRepository,
            IHotelRepository hotelRepository,
            IRoomRepository roomRepository,
            IRoomPhotoRepository roomPhotoRepository,
            ImageService imageService,
            IMapper mapper,
            IValidator<CreateRoomDTO> creatingRoomValidator
            )
        {
            _userRepository = userRepository;
            _hotelRepository = hotelRepository;
            _roomRepository = roomRepository;
            _roomPhotoRepository = roomPhotoRepository;
            _imageService = imageService;
            _mapper = mapper;
            _creatingRoomValidator = creatingRoomValidator;
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

            if(hotel == null)
            {
                throw new NullReferenceException("Отель не найден");
            }

            if (user == null || user.Id != hotel.OwnerId)
            {
                throw new SecurityException("Недостаточно прав на создание номера в этом отеле");
            }

            var room = _mapper.Map<Room>(creatingRoom);
            await _roomRepository.AddAsync(room, ct);
            return _mapper.Map<RoomDTO>(room);
        }
    }
}
