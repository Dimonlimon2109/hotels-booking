
using AutoMapper;
using FluentValidation;
using HotelsBooking.BLL.DTO;
using HotelsBooking.BLL.Interfaces;
using HotelsBooking.DAL.Entities;
using HotelsBooking.DAL.Interfaces;

namespace HotelsBooking.BLL.Services
{
    public class AmenityService : IAmenityService
    {
        private readonly IAmenityRepository _amenityRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateAmenityDTO> _creatingAmenityValidator;
        private readonly IValidator<UpdateAmenityDTO> _updateAmenityValidator;

        public AmenityService
            (
            IAmenityRepository amenityRepository,
            IMapper mapper,
            IValidator<CreateAmenityDTO> creatingAmenityValidator,
            IValidator<UpdateAmenityDTO> updateAmenityValidator
            )
        {
            _amenityRepository = amenityRepository;
            _mapper = mapper;
            _creatingAmenityValidator = creatingAmenityValidator;
            _updateAmenityValidator = updateAmenityValidator;
        }

        public async Task CreateAmenityAsync(CreateAmenityDTO creatingAmenity, CancellationToken ct = default)
        {
            var validationResult = await _creatingAmenityValidator.ValidateAsync(creatingAmenity, ct);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var amenity = _mapper.Map<Amenity>(creatingAmenity);
            await _amenityRepository.AddAsync(amenity, ct);
            await _amenityRepository.SaveChangesAsync(ct);
        }

        public async Task<IEnumerable<AmenityDTO>> GetAllAmenitiesAsync(CancellationToken ct = default)
        {
            var amenities = await _amenityRepository.GetAllAsync(ct);
            return amenities.Select(a => _mapper.Map<AmenityDTO>(a));
        }

        public async Task<AmenityDTO> GetAmenityByIdAsync(int id, CancellationToken ct = default)
        {
            var amenity = await _amenityRepository.GetByIdAsync(id, ct)
                ?? throw new NullReferenceException("Удобство не найдено");
            return _mapper.Map<AmenityDTO>(amenity);
        }

        public async Task DeleteAmenityAsync(int id, CancellationToken ct = default)
        {
            await _amenityRepository.DeleteAsync(id, ct);
            await _amenityRepository.SaveChangesAsync(ct);
        }

        public async Task UpdateAmenityAsync(int id, UpdateAmenityDTO updatingAmenity, CancellationToken ct = default)
        {
            var validationResult = await _updateAmenityValidator.ValidateAsync(updatingAmenity, ct);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var amenity = await _amenityRepository.GetByIdAsync(id, ct)
                ?? throw new NullReferenceException("Удобство не найдено");

            amenity.Name = updatingAmenity.Name;
            _amenityRepository.Update(amenity);
            await _amenityRepository.SaveChangesAsync(ct);
        }
    }
}
