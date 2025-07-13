
using HotelsBooking.DAL.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace HotelsBooking.DAL.Data.Configurations
{
    public class BookingConfiguration : IEntityTypeConfiguration<Booking>
    {
        public void Configure(EntityTypeBuilder<Booking> builder)
        {
            builder.HasKey(b => b.Id);

            builder.Property(b => b.CheckInDate)
                   .IsRequired();

            builder.Property(b => b.CheckOutDate)
                   .IsRequired();

            builder.Property(b => b.Adults)
                   .IsRequired();

            builder.Property(b => b.Children)
                   .IsRequired();

            builder.Property(b => b.TotalPrice)
                   .HasColumnType("decimal(10,2)")
                   .IsRequired();

            builder.Property(b => b.Status)
                   .HasConversion<string>()
                   .IsRequired();

            builder.HasOne(b => b.Room)
                   .WithMany(r => r.Bookings)
                   .HasForeignKey(b => b.RoomId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(b => b.User)
                   .WithMany(u => u.Bookings)
                   .HasForeignKey(b => b.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.Property(b => b.ChargeId)
                    .HasMaxLength(100)
                    .IsRequired(false);

            builder.Property(b => b.CancellationReason)
                   .HasMaxLength(500)
                   .IsUnicode()
                   .IsRequired(false);

            builder.Property(b => b.CancellationJobId)
                   .HasMaxLength(100)
                   .IsRequired(false);
        }
    }
}
