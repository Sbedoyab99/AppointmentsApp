using AppointmentsApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppointmentsApp.Infrastructure.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("Roles");

            builder.HasKey(role => role.Id);

            builder.Property(role => role.Name)
                .HasMaxLength(80)
                .IsRequired();

            builder.Property(role => role.NormalizedName)
                .HasMaxLength(80)
                .IsRequired();

            builder.Property(role => role.Description)
                .HasMaxLength(250)
                .IsRequired();

            builder.HasIndex(role => role.NormalizedName)
                .IsUnique();

            builder.HasData(
                new Role
                {
                    Id = 1,
                    Name = "Admin",
                    NormalizedName = "ADMIN",
                    Description = "Rol con acceso administrativo.",
                    IsActive = true
                },
                new Role
                {
                    Id = 2,
                    Name = "Staff",
                    NormalizedName = "STAFF",
                    Description = "Rol operativo con permisos limitados.",
                    IsActive = true
                }
            );
        }
    }
}
