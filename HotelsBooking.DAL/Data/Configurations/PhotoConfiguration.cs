using HotelsBooking.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelsBooking.DAL.Data.Configurations
{
    public class PhotoConfiguration : IEntityTypeConfiguration<PhotoBase>
    {
        public void Configure(EntityTypeBuilder<PhotoBase> builder)
        {
            builder
                .HasDiscriminator<string>("PhotoType")
                .HasValue<HotelPhoto>("Hotel");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.FilePath)
                .IsRequired()
                .HasMaxLength(200);
        }
    }
}
