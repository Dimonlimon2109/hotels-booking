
using HotelsBooking.DAL.Data.Configurations;
using HotelsBooking.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace HotelsBooking.DAL.Data
{
    public class ApplicationContext(DbContextOptions<ApplicationContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new UserConfiguration());
        }
    }
}
