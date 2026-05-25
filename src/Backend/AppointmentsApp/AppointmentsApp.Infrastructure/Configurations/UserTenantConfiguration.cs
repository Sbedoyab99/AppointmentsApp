using AppointmentsApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppointmentsApp.Infrastructure.Configurations
{
    public class UserTenantConfiguration : IEntityTypeConfiguration<UserTenant>
    {
        public void Configure(EntityTypeBuilder<UserTenant> builder)
        {
            builder.ToTable("UserTenants");

            builder.HasKey(userTenant => new { userTenant.UserId, userTenant.BusinessProfileId });

            builder.Property(userTenant => userTenant.AssignedAtUtc)
                .IsRequired();

            builder.HasIndex(userTenant => userTenant.BusinessProfileId);
            builder.HasIndex(userTenant => new { userTenant.BusinessProfileId, userTenant.IsOwner });

            builder.HasOne(userTenant => userTenant.User)
                .WithMany(user => user.UserTenants)
                .HasForeignKey(userTenant => userTenant.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(userTenant => userTenant.BusinessProfile)
                .WithMany(profile => profile.UserTenants)
                .HasForeignKey(userTenant => userTenant.BusinessProfileId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
