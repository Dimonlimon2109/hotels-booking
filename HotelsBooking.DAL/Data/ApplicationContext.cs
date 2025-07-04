
using HotelsBooking.DAL.Data.Configurations;
using HotelsBooking.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace HotelsBooking.DAL.Data
{
    public class ApplicationContext(DbContextOptions<ApplicationContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<PhotoBase> Photos { get; set; }
        public DbSet<HotelPhoto> HotelPhotos { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<RoomPhoto> roomPhotos { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new HotelConfiguration());
            modelBuilder.ApplyConfiguration(new PhotoConfiguration());
            modelBuilder.ApplyConfiguration(new HotelConfiguration());
            modelBuilder.ApplyConfiguration(new RoomConfiguration());
            modelBuilder.ApplyConfiguration(new RoomPhotoConfiguration());
        }
    }
}
