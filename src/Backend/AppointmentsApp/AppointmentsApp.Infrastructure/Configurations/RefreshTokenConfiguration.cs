using AppointmentsApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppointmentsApp.Infrastructure.Configurations
{
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.ToTable("RefreshTokens");

            builder.HasKey(refreshToken => refreshToken.Id);

            builder.Property(refreshToken => refreshToken.TokenHash)
                .HasMaxLength(256)
                .IsRequired();

            builder.Property(refreshToken => refreshToken.CreatedAtUtc)
                .IsRequired();

            builder.Property(refreshToken => refreshToken.ExpiresAtUtc)
                .IsRequired();

            builder.Property(refreshToken => refreshToken.ReplacedByTokenHash)
                .HasMaxLength(256)
                .IsRequired(false);

            builder.HasIndex(refreshToken => refreshToken.UserId);
            builder.HasIndex(refreshToken => refreshToken.ExpiresAtUtc);

            builder.HasOne(refreshToken => refreshToken.User)
                .WithMany(user => user.RefreshTokens)
                .HasForeignKey(refreshToken => refreshToken.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
