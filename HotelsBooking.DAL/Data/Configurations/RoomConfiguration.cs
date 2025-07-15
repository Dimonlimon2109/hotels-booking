
using HotelsBooking.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelsBooking.DAL.Data.Configurations
{
    public class RoomConfiguration : IEntityTypeConfiguration<Room>
    {
        public void Configure(EntityTypeBuilder<Room> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(r => r.Hotel)
                  .WithMany(h => h.Rooms)
                  .HasForeignKey(r => r.HotelId)
                  .OnDelete(DeleteBehavior.Cascade);

            builder.Property(x => x.Type)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(r => r.PricePerNight)
                   .HasColumnType("decimal(10,2)")
                   .IsRequired();

            builder.Property(r => r.Capacity)
                   .IsRequired();
        }
    }
}
