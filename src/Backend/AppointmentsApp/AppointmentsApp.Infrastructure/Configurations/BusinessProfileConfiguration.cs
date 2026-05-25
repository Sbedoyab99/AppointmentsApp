using AppointmentsApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppointmentsApp.Infrastructure.Configurations
{
    public class BusinessProfileConfiguration : IEntityTypeConfiguration<BusinessProfile>
    {
        public void Configure(EntityTypeBuilder<BusinessProfile> builder)
        {
            builder.ToTable("BusinessProfiles");

            builder.HasKey(profile => profile.Id);

            builder.Property(profile => profile.Name)
                .HasMaxLength(150)
                .IsRequired();

            builder.Property(profile => profile.TradeName)
                .HasMaxLength(150)
                .IsRequired();

            builder.Property(profile => profile.NormalizedTradeName)
                .HasMaxLength(150)
                .IsRequired();

            builder.Property(profile => profile.TimeZone)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(profile => profile.ContactEmail)
                .HasMaxLength(320)
                .IsRequired();

            builder.Property(profile => profile.CreatedAtUtc)
                .IsRequired();

            builder.Property(profile => profile.UpdatedAtUtc)
                .IsRequired();

            builder.HasIndex(profile => profile.NormalizedTradeName)
                .IsUnique();

            builder.HasIndex(profile => profile.ContactEmail);
        }
    }
}
