
using HotelsBooking.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelsBooking.DAL.Data.Configurations
{
    public class HotelConfiguration : IEntityTypeConfiguration<Hotel>
    {
        public void Configure(EntityTypeBuilder<Hotel> builder)
        {
            builder.HasKey(h => h.Id);

            builder.Property(h => h.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(h => h.Country)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(h => h.City)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(h => h.Street)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(h => h.HouseNumber)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(h => h.Latitude)
                .HasColumnType("decimal(9,6)")
                .IsRequired();

            builder.Property(h => h.Longitude)
                .HasColumnType("decimal(9,6)")
                .IsRequired();

            builder.Property(h => h.StarRating)
                .IsRequired();

            builder.Property(h => h.ReviewRating)
                .IsRequired()
                .HasDefaultValue(0)
                .HasPrecision(3, 2);

            builder.Property(h => h.Description)
                .IsRequired()
                .HasMaxLength(1000);

            builder.Property(h => h.OwnerId)
                .IsRequired();

            builder.HasOne(h => h.Owner)
                .WithMany(o => o.Hotels)
                .HasForeignKey(h => h.OwnerId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
