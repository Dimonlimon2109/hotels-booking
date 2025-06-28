
using AutoMapper;
using FluentValidation;
using HotelsBooking.BLL.DTO;
using HotelsBooking.DAL.Entities;
using HotelsBooking.DAL.Interfaces;

namespace HotelsBooking.BLL.Services
{
    public class HotelService
    {
        private readonly IHotelRepository _hotelRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateHotelDTO> _creatingHotelValidator;

        public HotelService(
            IHotelRepository hotelRepository,
            IMapper mapper,
            IValidator<CreateHotelDTO> creatingHotelValidator
            )
        {
            _hotelRepository = hotelRepository;
            _mapper = mapper;
            _creatingHotelValidator = creatingHotelValidator;
        }

        public async Task<HotelDTO> CreateHotelAsync(CreateHotelDTO creatingHotel, CancellationToken ct = default)
        {
            var validationResult = _creatingHotelValidator.Validate(creatingHotel);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var hotel = _mapper.Map<Hotel>(creatingHotel);
            
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
    }
}
