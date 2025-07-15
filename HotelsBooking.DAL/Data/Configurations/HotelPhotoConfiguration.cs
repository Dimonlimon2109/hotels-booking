
using HotelsBooking.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelsBooking.DAL.Data.Configurations
{
    public class HotelPhotoConfiguration: IEntityTypeConfiguration<HotelPhoto>
    {
        public void Configure(EntityTypeBuilder<HotelPhoto> builder)
        {
            builder.HasOne(hp => hp.Hotel)
                   .WithMany(h => h.Photos)
                   .HasForeignKey(hp => hp.HotelId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
