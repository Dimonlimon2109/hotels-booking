using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using HotelsBooking.BLL.DTO;
using HotelsBooking.BLL.Interfaces;
using HotelsBooking.BLL.Models;
using HotelsBooking.BLL.Services;
using HotelsBooking.BLL.Validators;
using HotelsBooking.DAL.Entities;
using HotelsBooking.DAL.Interfaces;
using Moq;
using System.Security;

namespace HotelsBooking.Tests
{
    public class HotelServiceTests
    {
        private readonly Mock<IHotelRepository> _mockHotelRepository;
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IValidator<CreateHotelDTO>> _mockCreatingHotelValidator;
        private readonly Mock<IValidator<UpdateHotelDTO>> _mockUpdatingHotelValidator;
        private readonly Mock<IImageService> _mockImageService;
        private readonly Mock<IHotelPhotoRepository> _mockHotelPhotoRepository;
        private readonly Mock<IAmenityRepository> _mockAmenityRepository;
        private readonly HotelService _hotelService;

        public HotelServiceTests()
        {
            _mockHotelRepository = new Mock<IHotelRepository>();
            _mockUserRepository = new Mock<IUserRepository>();
            _mockMapper = new Mock<IMapper>();
            _mockCreatingHotelValidator = new Mock<IValidator<CreateHotelDTO>>();
            _mockUpdatingHotelValidator = new Mock<IValidator<UpdateHotelDTO>>();
            _mockImageService = new Mock<IImageService>();
            _mockHotelPhotoRepository = new Mock<IHotelPhotoRepository>();
            _mockAmenityRepository = new Mock<IAmenityRepository>();

            _hotelService = new HotelService(
                _mockHotelRepository.Object,
                _mockUserRepository.Object,
                _mockMapper.Object,
                _mockCreatingHotelValidator.Object,
                _mockUpdatingHotelValidator.Object,
                _mockImageService.Object,
                _mockHotelPhotoRepository.Object,
                _mockAmenityRepository.Object
            );
        }

        [Fact]
        public async Task CreateHotelAsync_ValidData_ReturnsHotelDTO()
        {
            var userEmail = "test@example.com";
            var createHotelDTO = new CreateHotelDTO
            {
                Name = "Test Hotel",
                Country = "Test Country",
                City = "Test City",
                Street = "Test Street",
                HouseNumber = "123",
                Latitude = 55.7558,
                Longitude = 37.6176,
                StarRating = 4,
                Description = "Test Description",
                AmenityIds = new List<int> { 1, 2 }
            };

            var user = new User { Id = 1, Email = userEmail };
            var amenities = new List<Amenity>
            {
                new Amenity { Id = 1, Name = "WiFi" },
                new Amenity { Id = 2, Name = "Pool" }
            };
            var hotel = new Hotel { Id = 1, Name = "Test Hotel", OwnerId = 1 };
            var hotelDTO = new HotelDTO { Id = 1, Name = "Test Hotel" };

            var validationResult = new ValidationResult();
            _mockCreatingHotelValidator.Setup(x => x.Validate(createHotelDTO))
                .Returns(validationResult);
            _mockUserRepository.Setup(x => x.GetByEmailAsync(userEmail, It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);
            _mockAmenityRepository.Setup(x => x.GetExistingAmenitiesAsync(createHotelDTO.AmenityIds, It.IsAny<CancellationToken>()))
                .ReturnsAsync(amenities);
            _mockMapper.Setup(x => x.Map<Hotel>(createHotelDTO))
                .Returns(hotel);
            _mockMapper.Setup(x => x.Map<HotelDTO>(hotel))
                .Returns(hotelDTO);

            var result = await _hotelService.CreateHotelAsync(userEmail, createHotelDTO);

            Assert.NotNull(result);
            Assert.Equal(hotelDTO.Id, result.Id);
            Assert.Equal(hotelDTO.Name, result.Name);
            _mockHotelRepository.Verify(x => x.AddAsync(It.IsAny<Hotel>(), createHotelDTO.Latitude, createHotelDTO.Longitude, It.IsAny<CancellationToken>()), Times.Once);
            _mockHotelRepository.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task CreateHotelAsync_InvalidValidation_ThrowsValidationException()
        {
            var userEmail = "test@example.com";
            var createHotelDTO = new CreateHotelDTO
            {
                Name = "",
                Country = "Test Country",
                City = "Test City",
                Street = "Test Street",
                HouseNumber = "123",
                Latitude = 55.7558,
                Longitude = 37.6176,
                StarRating = 4,
                Description = "Test Description",
                AmenityIds = new List<int> { 1, 2 }
            };

            var validationResult = new ValidationResult(new List<ValidationFailure>
            {
                new ValidationFailure("Name", "Name is required")
            });

            _mockCreatingHotelValidator.Setup(x => x.Validate(createHotelDTO))
                .Returns(validationResult);

            await Assert.ThrowsAsync<ValidationException>(() => 
                _hotelService.CreateHotelAsync(userEmail, createHotelDTO));
        }

        [Fact]
        public async Task CreateHotelAsync_UserNotFound_ThrowsSecurityException()
        {
            var userEmail = "nonexistent@example.com";
            var createHotelDTO = new CreateHotelDTO
            {
                Name = "Test Hotel",
                Country = "Test Country",
                City = "Test City",
                Street = "Test Street",
                HouseNumber = "123",
                Latitude = 55.7558,
                Longitude = 37.6176,
                StarRating = 4,
                Description = "Test Description",
                AmenityIds = new List<int> { 1, 2 }
            };

            var validationResult = new ValidationResult();
            _mockCreatingHotelValidator.Setup(x => x.Validate(createHotelDTO))
                .Returns(validationResult);
            _mockUserRepository.Setup(x => x.GetByEmailAsync(userEmail, It.IsAny<CancellationToken>()))
                .ReturnsAsync((User)null);

            await Assert.ThrowsAsync<SecurityException>(() => 
                _hotelService.CreateHotelAsync(userEmail, createHotelDTO));
        }

        [Fact]
        public async Task CreateHotelAsync_InvalidAmenityIds_ThrowsArgumentException()
        {
            var userEmail = "test@example.com";
            var createHotelDTO = new CreateHotelDTO
            {
                Name = "Test Hotel",
                Country = "Test Country",
                City = "Test City",
                Street = "Test Street",
                HouseNumber = "123",
                Latitude = 55.7558,
                Longitude = 37.6176,
                StarRating = 4,
                Description = "Test Description",
                AmenityIds = new List<int> { 1, 2, 999 }
            };

            var user = new User { Id = 1, Email = userEmail };
            var amenities = new List<Amenity>
            {
                new Amenity { Id = 1, Name = "WiFi" },
                new Amenity { Id = 2, Name = "Pool" }
            };

            var validationResult = new ValidationResult();
            _mockCreatingHotelValidator.Setup(x => x.Validate(createHotelDTO))
                .Returns(validationResult);
            _mockUserRepository.Setup(x => x.GetByEmailAsync(userEmail, It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);
            _mockAmenityRepository.Setup(x => x.GetExistingAmenitiesAsync(createHotelDTO.AmenityIds, It.IsAny<CancellationToken>()))
                .ReturnsAsync(amenities);

            var exception = await Assert.ThrowsAsync<ArgumentException>(() => 
                _hotelService.CreateHotelAsync(userEmail, createHotelDTO));
            Assert.Contains("Некорректные AmenityIds", exception.Message);
        }

        [Fact]
        public async Task GetAllHotelsAsync_ValidFilters_ReturnsHotels()
        {
            var filters = new HotelFiltersModel(
                Name: "Test",
                City: "Moscow",
                StarRating: 4,
                AmenityIds: new List<int> { 1 },
                CenterLatitude: 55.7558,
                CenterLongitude: 37.6176,
                Limit: 10,
                Offset: 0,
                SortBy: "reviewRating",
                Order: "desc"
            );

            var hotels = new List<Hotel>
            {
                new Hotel { Id = 1, Name = "Test Hotel 1", StarRating = 4 },
                new Hotel { Id = 2, Name = "Test Hotel 2", StarRating = 5 }
            };

            var hotelsDTO = new List<HotelDTO>
            {
                new HotelDTO { Id = 1, Name = "Test Hotel 1", StarRating = 4 },
                new HotelDTO { Id = 2, Name = "Test Hotel 2", StarRating = 5 }
            };

            _mockHotelRepository.Setup(x => x.GetAllHotelsWithFiltersAsync(
                filters.Limit,
                filters.Offset,
                filters.Name,
                filters.City,
                filters.StarRating,
                filters.AmenityIds,
                filters.CenterLatitude,
                filters.CenterLongitude,
                filters.SortBy,
                filters.Order,
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(hotels);

            _mockMapper.Setup(x => x.Map<HotelDTO>(It.IsAny<Hotel>()))
                .Returns<Hotel>(h => hotelsDTO.First(dto => dto.Id == h.Id));

            var result = await _hotelService.GetAllHotelsAsync(filters);

            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetHotelAsync_ValidId_ReturnsHotelDTO()
        {
            var hotelId = 1;
            var hotel = new Hotel { Id = 1, Name = "Test Hotel", StarRating = 4 };
            var hotelDTO = new HotelDTO { Id = 1, Name = "Test Hotel", StarRating = 4 };

            _mockHotelRepository.Setup(x => x.GetHotelAsync(hotelId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(hotel);
            _mockMapper.Setup(x => x.Map<HotelDTO>(hotel))
                .Returns(hotelDTO);

            var result = await _hotelService.GetHotelAsync(hotelId);

            Assert.NotNull(result);
            Assert.Equal(hotelDTO.Id, result.Id);
            Assert.Equal(hotelDTO.Name, result.Name);
        }

        [Fact]
        public async Task GetMyHotelsAsync_ValidUserEmail_ReturnsUserHotels()
        {
            var userEmail = "owner@example.com";
            var user = new User { Id = 1, Email = userEmail };
            var userHotels = new List<Hotel>
            {
                new Hotel { Id = 1, Name = "My Hotel 1", OwnerId = 1 },
                new Hotel { Id = 2, Name = "My Hotel 2", OwnerId = 1 }
            };

            var hotelsDTO = new List<HotelDTO>
            {
                new HotelDTO { Id = 1, Name = "My Hotel 1" },
                new HotelDTO { Id = 2, Name = "My Hotel 2" }
            };

            _mockUserRepository.Setup(x => x.GetByEmailAsync(userEmail, It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);
            _mockHotelRepository.Setup(x => x.GetOwnerHotelsAsync(user.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(userHotels);
            _mockMapper.Setup(x => x.Map<HotelDTO>(It.IsAny<Hotel>()))
                .Returns<Hotel>(h => hotelsDTO.First(dto => dto.Id == h.Id));

            var result = await _hotelService.GetMyHotelsAsync(userEmail);

            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetMyHotelsAsync_UserNotFound_ThrowsSecurityException()
        {
            var userEmail = "nonexistent@example.com";

            _mockUserRepository.Setup(x => x.GetByEmailAsync(userEmail, It.IsAny<CancellationToken>()))
                .ReturnsAsync((User)null);

            await Assert.ThrowsAsync<SecurityException>(() => 
                _hotelService.GetMyHotelsAsync(userEmail));
        }

        [Fact]
        public async Task DeleteHotelAsync_ValidOwner_DeletesHotel()
        {
            var hotelId = 1;
            var userEmail = "owner@example.com";
            var user = new User { Id = 1, Email = userEmail };
            var hotel = new Hotel { Id = 1, Name = "Test Hotel", OwnerId = 1 };

            _mockUserRepository.Setup(x => x.GetByEmailAsync(userEmail, It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);
            _mockHotelRepository.Setup(x => x.GetHotelByIdWithOwnerAsync(hotelId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(hotel);

            await _hotelService.DeleteHotelAsync(hotelId, userEmail);

            _mockHotelRepository.Verify(x => x.DeleteAsync(hotelId, It.IsAny<CancellationToken>()), Times.Once);
            _mockHotelRepository.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteHotelAsync_NotOwner_ThrowsSecurityException()
        {
            var hotelId = 1;
            var userEmail = "notowner@example.com";
            var user = new User { Id = 2, Email = userEmail };
            var hotel = new Hotel { Id = 1, Name = "Test Hotel", OwnerId = 1 };

            _mockUserRepository.Setup(x => x.GetByEmailAsync(userEmail, It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);
            _mockHotelRepository.Setup(x => x.GetHotelByIdWithOwnerAsync(hotelId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(hotel);

            await Assert.ThrowsAsync<SecurityException>(() => 
                _hotelService.DeleteHotelAsync(hotelId, userEmail));
        }

        [Fact]
        public async Task UpdateHotelAsync_ValidOwner_UpdatesHotel()
        {
            var hotelId = 1;
            var userEmail = "owner@example.com";
            var updateHotelDTO = new UpdateHotelDTO
            {
                Name = "Updated Hotel",
                Country = "Updated Country",
                City = "Updated City",
                Street = "Updated Street",
                HouseNumber = "456",
                Latitude = 55.7558,
                Longitude = 37.6176,
                StarRating = 5,
                Description = "Updated Description",
                AmenityIds = new List<int> { 1, 2 }
            };

            var user = new User { Id = 1, Email = userEmail };
            var hotel = new Hotel { Id = 1, Name = "Old Hotel", OwnerId = 1 };
            var amenities = new List<Amenity>
            {
                new Amenity { Id = 1, Name = "WiFi" },
                new Amenity { Id = 2, Name = "Pool" }
            };

            var validationResult = new ValidationResult();
            _mockUpdatingHotelValidator.Setup(x => x.Validate(updateHotelDTO))
                .Returns(validationResult);
            _mockHotelRepository.Setup(x => x.GetHotelByIdWithOwnerAsync(hotelId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(hotel);
            _mockUserRepository.Setup(x => x.GetByEmailAsync(userEmail, It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);
            _mockAmenityRepository.Setup(x => x.GetExistingAmenitiesAsync(updateHotelDTO.AmenityIds, It.IsAny<CancellationToken>()))
                .ReturnsAsync(amenities);

            await _hotelService.UpdateHotelAsync(hotelId, userEmail, updateHotelDTO);

            Assert.Equal(updateHotelDTO.Name, hotel.Name);
            Assert.Equal(updateHotelDTO.Country, hotel.Country);
            Assert.Equal(updateHotelDTO.City, hotel.City);
            Assert.Equal(updateHotelDTO.Street, hotel.Street);
            Assert.Equal(updateHotelDTO.HouseNumber, hotel.HouseNumber);
            Assert.Equal(updateHotelDTO.Description, hotel.Description);
            Assert.Equal(updateHotelDTO.StarRating, hotel.StarRating);
            _mockHotelRepository.Verify(x => x.Update(hotel, updateHotelDTO.Latitude, updateHotelDTO.Longitude), Times.Once);
            _mockHotelRepository.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateHotelAsync_HotelNotFound_ThrowsNullReferenceException()
        {
            var hotelId = 999;
            var userEmail = "owner@example.com";
            var updateHotelDTO = new UpdateHotelDTO
            {
                Name = "Updated Hotel",
                Country = "Updated Country",
                City = "Updated City",
                Street = "Updated Street",
                HouseNumber = "456",
                Latitude = 55.7558,
                Longitude = 37.6176,
                StarRating = 5,
                Description = "Updated Description",
                AmenityIds = new List<int> { 1, 2 }
            };

            _mockHotelRepository.Setup(x => x.GetHotelByIdWithOwnerAsync(hotelId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Hotel)null);

            await Assert.ThrowsAsync<NullReferenceException>(() => 
                _hotelService.UpdateHotelAsync(hotelId, userEmail, updateHotelDTO));
        }

        [Fact]
        public async Task UploadHotelPhotoAsync_ValidOwner_UploadsPhoto()
        {
            var hotelId = 1;
            var userEmail = "owner@example.com";
            var hotel = new Hotel { Id = 1, Name = "Test Hotel", OwnerId = 1 };
            var mockImage = new Mock<IImageFile>();
            var imagePath = "hotels/test-image.jpg";

            _mockHotelRepository.Setup(x => x.GetByIdAsync(hotelId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(hotel);
            _mockUserRepository.Setup(x => x.GetByEmailAsync(userEmail, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new User { Id = 1, Email = userEmail });
            _mockImageService.Setup(x => x.UploadImageAsync(mockImage.Object, "hotels", It.IsAny<CancellationToken>()))
                .ReturnsAsync(imagePath);

            await _hotelService.UploadHotelPhotoAsync(hotelId, mockImage.Object, userEmail);

            _mockImageService.Verify(x => x.UploadImageAsync(mockImage.Object, "hotels", It.IsAny<CancellationToken>()), Times.Once);
            _mockHotelPhotoRepository.Verify(x => x.AddAsync(It.Is<HotelPhoto>(p => p.FilePath == imagePath && p.HotelId == hotelId), It.IsAny<CancellationToken>()), Times.Once);
            _mockHotelRepository.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteHotelPhotoAsync_ValidOwner_DeletesPhoto()
        {
            var hotelId = 1;
            var photoId = 1;
            var userEmail = "owner@example.com";
            var hotel = new Hotel { Id = 1, Name = "Test Hotel", OwnerId = 1 };
            var photo = new HotelPhoto { Id = 1, FilePath = "hotels/test-image.jpg", HotelId = 1 };

            _mockHotelRepository.Setup(x => x.GetByIdAsync(hotelId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(hotel);
            _mockUserRepository.Setup(x => x.GetByEmailAsync(userEmail, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new User { Id = 1, Email = userEmail });
            _mockHotelPhotoRepository.Setup(x => x.GetByIdAsync(photoId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(photo);

            await _hotelService.DeleteHotelPhotoAsync(hotelId, photoId, userEmail);

            _mockImageService.Verify(x => x.DeleteImageAsync(photo.FilePath), Times.Once);
            _mockHotelPhotoRepository.Verify(x => x.DeleteAsync(photoId, It.IsAny<CancellationToken>()), Times.Once);
            _mockHotelRepository.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetTotalPagesAsync_ValidFilters_ReturnsCount()
        {
            var filters = new HotelFiltersModel(
                Name: "Test",
                City: "Moscow",
                StarRating: 4,
                AmenityIds: new List<int> { 1 },
                CenterLatitude: null,
                CenterLongitude: null,
                Limit: 10,
                Offset: 0,
                SortBy: null,
                Order: "asc"
            );

            var expectedCount = 25;

            _mockHotelRepository.Setup(x => x.GetHotelsTotalCountAsync(
                filters.Name,
                filters.City,
                filters.StarRating,
                filters.AmenityIds,
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedCount);

            var result = await _hotelService.GetTotalPagesAsync(filters);

            Assert.Equal(expectedCount, result);
        }
    }
} 