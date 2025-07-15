
using HotelsBooking.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelsBooking.DAL.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(100);
            builder.HasIndex(u => u.Email)
                .IsUnique();
            builder.Property(u => u.UserName)
                .IsRequired()
                .HasMaxLength(50);
            builder.HasIndex(u => u.UserName)
                .IsUnique();
            builder.Property(u => u.Role)
                .IsRequired();
            builder.Property(u => u.PasswordHash)
                .IsRequired()
                .HasMaxLength(256);
        }
    }
}
