using AppointmentsApp.Domain.Entities;
using AppointmentsApp.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppointmentsApp.Infrastructure.Configurations
{
    public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(user => user.Id);

            builder.Property(user => user.Email)
                .HasMaxLength(320)
                .IsRequired();

            builder.Property(user => user.NormalizedEmail)
                .HasMaxLength(320)
                .IsRequired();

            builder.Property(user => user.PasswordHash)
                .HasMaxLength(512)
                .IsRequired();

            builder.Property(user => user.FirstName)
                .HasMaxLength(120)
                .IsRequired();

            builder.Property(user => user.LastName)
                .HasMaxLength(120)
                .IsRequired();

            builder.Property(user => user.Status)
                .HasConversion<string>()
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(user => user.CreatedAtUtc)
                .IsRequired();

            builder.Property(user => user.UpdatedAtUtc)
                .IsRequired();

            builder.HasIndex(user => user.Email)
                .IsUnique();

            builder.HasIndex(user => user.NormalizedEmail)
                .IsUnique();
        }
    }
}
