using HotelsBooking.BLL.Models;
using HotelsBooking.DAL.Data;
using HotelsBooking.DAL.Entities;
using HotelsBooking.DAL.Interfaces;
using HotelsBooking.DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Moq;
using NetTopologySuite.Geometries;

namespace HotelsBooking.Tests
{
    public class HotelRepositoryTests : IDisposable
    {
        private readonly ApplicationContext _context;
        private readonly HotelRepository _repository;

        public HotelRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationContext(options);
            _repository = new HotelRepository(_context);
            
            SeedTestData();
        }

        private void SeedTestData()
        {
            var users = new List<User>
            {
                new User { Id = 1, UserName = "user1", Email = "user1@example.com", PasswordHash = "hash1" },
                new User { Id = 2, UserName = "user2", Email = "user2@example.com", PasswordHash = "hash2" }
            };

            var amenities = new List<Amenity>
            {
                new Amenity { Id = 1, Name = "WiFi" },
                new Amenity { Id = 2, Name = "Pool" },
                new Amenity { Id = 3, Name = "Gym" }
            };

            var hotels = new List<Hotel>
            {
                new Hotel
                {
                    Id = 1,
                    Name = "Moscow Hotel",
                    Country = "Russia",
                    City = "Moscow",
                    Street = "Tverskaya",
                    HouseNumber = "1",
                    StarRating = 4,
                    ReviewRating = 4.5,
                    Description = "Luxury hotel in Moscow",
                    OwnerId = 1,
                    Location = new Point(37.6176, 55.7558) { SRID = 4326 },
                    Amenities = new List<Amenity> { amenities[0], amenities[1] }
                },
                new Hotel
                {
                    Id = 2,
                    Name = "SPB Hotel",
                    Country = "Russia",
                    City = "Saint Petersburg",
                    Street = "Nevsky Prospect",
                    HouseNumber = "10",
                    StarRating = 5,
                    ReviewRating = 4.8,
                    Description = "Premium hotel in SPB",
                    OwnerId = 1,
                    Location = new Point(30.3141, 59.9386) { SRID = 4326 },
                    Amenities = new List<Amenity> { amenities[0], amenities[2] }
                },
                new Hotel
                {
                    Id = 3,
                    Name = "Kazan Hotel",
                    Country = "Russia",
                    City = "Kazan",
                    Street = "Baumana",
                    HouseNumber = "5",
                    StarRating = 3,
                    ReviewRating = 4.2,
                    Description = "Comfortable hotel in Kazan",
                    OwnerId = 2,
                    Location = new Point(49.1234, 55.7890) { SRID = 4326 },
                    Amenities = new List<Amenity> { amenities[0] }
                }
            };

            _context.Users.AddRange(users);
            _context.Amenities.AddRange(amenities);
            _context.Hotels.AddRange(hotels);
            _context.SaveChanges();
        }

        [Fact]
        public async Task GetHotelAsync_ValidId_ReturnsHotel()
        {
            var hotelId = 1;

            var result = await _repository.GetHotelAsync(hotelId);

            Assert.NotNull(result);
            Assert.Equal(hotelId, result.Id);
            Assert.Equal("Moscow Hotel", result.Name);
            Assert.Equal("Russia", result.Country);
            Assert.Equal("Moscow", result.City);
            Assert.Equal(4, result.StarRating);
            Assert.Equal(4.5, result.ReviewRating);
        }

        [Fact]
        public async Task GetHotelAsync_InvalidId_ReturnsNull()
        {
            // Arrange
            var hotelId = 999;

            // Act
            var result = await _repository.GetHotelAsync(hotelId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetHotelByIdWithOwnerAsync_ValidId_ReturnsHotelWithOwner()
        {
            var hotelId = 1;

            var result = await _repository.GetHotelByIdWithOwnerAsync(hotelId);

            Assert.NotNull(result);
            Assert.Equal(hotelId, result.Id);
            Assert.NotNull(result.Owner);
            Assert.Equal(1, result.Owner.Id);
            Assert.Equal("user1@example.com", result.Owner.Email);
        }

        [Fact]
        public async Task GetOwnerHotelsAsync_ValidOwnerId_ReturnsOwnerHotels()
        {
            var ownerId = 1;

            var result = await _repository.GetOwnerHotelsAsync(ownerId);

            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.All(result, hotel => Assert.Equal(ownerId, hotel.OwnerId));
            Assert.Contains(result, h => h.Name == "Moscow Hotel");
            Assert.Contains(result, h => h.Name == "SPB Hotel");
        }

        [Fact]
        public async Task GetOwnerHotelsAsync_InvalidOwnerId_ReturnsEmpty()
        {
            var ownerId = 999;

            var result = await _repository.GetOwnerHotelsAsync(ownerId);

            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetAllHotelsWithFiltersAsync_ValidFilters_ReturnsFilteredHotels()
        {
            var filters = new HotelFiltersModel(
                Name: "Moscow",
                City: "Moscow",
                StarRating: 4,
                AmenityIds: null,
                CenterLatitude: 55.7558,
                CenterLongitude: 37.6176,
                Limit: 10,
                Offset: 0,
                SortBy: "reviewRating",
                Order: "desc"
            );

            var result = await _repository.GetAllHotelsWithFiltersAsync(
                filters.Limit,
                filters.Offset,
                filters.Name,
                filters.City,
                filters.StarRating,
                filters.AmenityIds,
                filters.CenterLatitude,
                filters.CenterLongitude,
                filters.SortBy,
                filters.Order
            );

            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal("Moscow Hotel", result.First().Name);
        }

        [Fact]
        public async Task GetAllHotelsWithFiltersAsync_NoFilters_ReturnsAllHotels()
        {
            var filters = new HotelFiltersModel(
                Name: null,
                City: null,
                StarRating: null,
                AmenityIds: null,
                CenterLatitude: null,
                CenterLongitude: null,
                Limit: 10,
                Offset: 0,
                SortBy: null,
                Order: "asc"
            );

            var result = await _repository.GetAllHotelsWithFiltersAsync(
                filters.Limit,
                filters.Offset,
                filters.Name,
                filters.City,
                filters.StarRating,
                filters.AmenityIds,
                filters.CenterLatitude,
                filters.CenterLongitude,
                filters.SortBy,
                filters.Order
            );

            Assert.NotNull(result);
            Assert.Equal(3, result.Count());
        }

        [Fact]
        public async Task GetHotelsTotalCountAsync_ValidFilters_ReturnsCount()
        {
            var name = "Moscow";
            var city = "Moscow";
            var starRating = 4;
            IEnumerable<int>? amenityIds = null;

            var result = await _repository.GetHotelsTotalCountAsync(name, city, starRating, amenityIds);

            Assert.Equal(1, result);
        }

        [Fact]
        public async Task GetHotelsTotalCountAsync_NoFilters_ReturnsTotalCount()
        {
            string? name = null;
            string? city = null;
            int? starRating = null;
            IEnumerable<int>? amenityIds = null;

            var result = await _repository.GetHotelsTotalCountAsync(name, city, starRating, amenityIds);

            Assert.Equal(3, result);
        }

        [Fact]
        public async Task AddAsync_ValidHotel_AddsHotelToContext()
        {
            var hotel = new Hotel
            {
                Name = "New Hotel",
                Country = "Russia",
                City = "Sochi",
                Street = "Kurortny",
                HouseNumber = "15",
                StarRating = 4,
                ReviewRating = 4.3,
                Description = "New hotel in Sochi",
                OwnerId = 1
            };

            var latitude = 39.7233;
            var longitude = 43.5855;

            await _repository.AddAsync(hotel, latitude, longitude);
            await _repository.SaveChangesAsync();

            var addedHotel = await _context.Hotels.FirstOrDefaultAsync(h => h.Name == "New Hotel");
            Assert.NotNull(addedHotel);
            Assert.Equal("New Hotel", addedHotel.Name);
            Assert.Equal("Russia", addedHotel.Country);
            Assert.Equal("Sochi", addedHotel.City);
            Assert.Equal(4, addedHotel.StarRating);
            Assert.NotNull(addedHotel.Location);
            Assert.Equal(longitude, addedHotel.Location.X);
            Assert.Equal(latitude, addedHotel.Location.Y);
        }

        [Fact]
        public async Task DeleteAsync_ValidId_RemovesHotelFromContext()
        {
            var hotelId = 1;
            var initialCount = await _context.Hotels.CountAsync();

            await _repository.DeleteAsync(hotelId);
            await _repository.SaveChangesAsync();

            var finalCount = await _context.Hotels.CountAsync();
            Assert.Equal(initialCount - 1, finalCount);
            
            var deletedHotel = await _context.Hotels.FindAsync(hotelId);
            Assert.Null(deletedHotel);
        }

        [Fact]
        public async Task DeleteAsync_InvalidId_DoesNothing()
        {
            var hotelId = 999;
            var initialCount = await _context.Hotels.CountAsync();

            await _repository.DeleteAsync(hotelId);
            await _repository.SaveChangesAsync();

            var finalCount = await _context.Hotels.CountAsync();
            Assert.Equal(initialCount, finalCount);
        }

        [Fact]
        public async Task SaveChangesAsync_CallsContextSaveChanges()
        {
             
            var hotel = new Hotel
            {
                Name = "Test Hotel",
                Country = "Russia",
                City = "Test City",
                Street = "Test Street",
                HouseNumber = "1",
                StarRating = 3,
                Description = "Test Description",
                OwnerId = 1,
                Location = new Point(37.6176, 55.7558) { SRID = 4326 } // Добавляем Location
            };

            _context.Hotels.Add(hotel);

            await _repository.SaveChangesAsync();

            var savedHotel = await _context.Hotels.FirstOrDefaultAsync(h => h.Name == "Test Hotel");
            Assert.NotNull(savedHotel);
        }

        [Fact]
        public async Task Update_ValidHotel_UpdatesHotelInContext()
        {
            var hotelId = 1;
            var hotel = await _context.Hotels.FindAsync(hotelId);
            Assert.NotNull(hotel);

            var originalName = hotel.Name;
            var originalStarRating = hotel.StarRating;

            hotel.Name = "Updated Hotel Name";
            hotel.StarRating = 5;
            _repository.Update(hotel, 37.6176, 55.7558);
            await _repository.SaveChangesAsync();

            var updatedHotel = await _context.Hotels.FindAsync(hotelId);
            Assert.NotNull(updatedHotel);
            Assert.Equal("Updated Hotel Name", updatedHotel.Name);
            Assert.Equal(5, updatedHotel.StarRating);
            Assert.NotEqual(originalName, updatedHotel.Name);
            Assert.NotEqual(originalStarRating, updatedHotel.StarRating);
        }

        [Fact]
        public async Task GetByIdAsync_ValidId_ReturnsHotel()
        {
            var hotelId = 1;

            var result = await _repository.GetByIdAsync(hotelId);

            Assert.NotNull(result);
            Assert.Equal(hotelId, result.Id);
            Assert.Equal("Moscow Hotel", result.Name);
        }

        [Fact]
        public async Task GetByIdAsync_InvalidId_ReturnsNull()
        {
            var hotelId = 999;

            var result = await _repository.GetByIdAsync(hotelId);

            Assert.Null(result);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllHotels()
        {
            var result = await _repository.GetAllAsync();

            Assert.NotNull(result);
            Assert.Equal(3, result.Count());
            Assert.Contains(result, h => h.Name == "Moscow Hotel");
            Assert.Contains(result, h => h.Name == "SPB Hotel");
            Assert.Contains(result, h => h.Name == "Kazan Hotel");
        }

        [Fact]
        public async Task GetHotelsAsync_ReturnsAllHotels()
        {
            var result = await _repository.GetHotelsAsync();

            Assert.NotNull(result);
            Assert.Equal(3, result.Count());
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
} 