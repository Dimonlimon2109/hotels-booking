
using HotelsBooking.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelsBooking.DAL.Data.Configurations
{
    public class RoomPhotoConfiguration: IEntityTypeConfiguration<RoomPhoto>
    {
        public void Configure(EntityTypeBuilder<RoomPhoto> builder)
        {
            builder.HasOne(hp => hp.Room)
                   .WithMany(h => h.Photos)
                   .HasForeignKey(rp => rp.RoomId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
