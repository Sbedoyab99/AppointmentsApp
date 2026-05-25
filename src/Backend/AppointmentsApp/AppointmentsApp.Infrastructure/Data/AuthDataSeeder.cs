using AppointmentsApp.Application.Helpers;
using AppointmentsApp.Domain.Entities;
using AppointmentsApp.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AppointmentsApp.Infrastructure.Data
{
    public static class AuthDataSeeder
    {
        // IDs fijos para datos de prueba
        public static class SeedIds
        {
            public static readonly Guid AdminUserId1 = new("10000000-0000-0000-0000-000000000001");
            public static readonly Guid AdminUserId2 = new("10000000-0000-0000-0000-000000000002");

            public static readonly Guid BusinessProfileId1 = new("10000000-0000-0000-0000-000000000100");
            public static readonly Guid BusinessProfileId2 = new("10000000-0000-0000-0000-000000000101");

            public static readonly int AdminRoleId = 1;
        }

        public static async Task SeedAsync(DataContext context, ILoggerFactory loggerFactory)
        {
            ILogger logger = loggerFactory.CreateLogger("AuthDataSeeder");

            try
            {
                // Seed Roles si no existen
                if (!await context.Roles.AnyAsync())
                {
                    context.Roles.AddRange(
                        new Role { Id = 1, Name = "Admin", Description = "Administrador del sistema", IsActive = true },
                        new Role { Id = 2, Name = "Staff", Description = "Personal del negocio", IsActive = true }
                    );
                    await context.SaveChangesAsync();
                    logger.LogInformation("Roles semilla agregados");
                }

                // Seed Users si no existen
                if (!await context.Users.AnyAsync())
                {
                    var users = new List<AppUser>
                    {
                        new()
                        {
                            Id = SeedIds.AdminUserId1,
                            Email = "admin@business1.com",
                            NormalizedEmail = "admin@business1.com".ToLower(),
                            PasswordHash = PasswordHashHelper.HashPassword("Admin123456"),
                            FirstName = "Admin",
                            LastName = "Uno",
                            Status = UserStatus.Active,
                            IsActive = true,
                            CreatedAtUtc = DateTimeOffset.UtcNow,
                            UpdatedAtUtc = DateTimeOffset.UtcNow
                        },
                        new()
                        {
                            Id = SeedIds.AdminUserId2,
                            Email = "admin@business2.com",
                            NormalizedEmail = "admin@business2.com".ToLower(),
                            PasswordHash = PasswordHashHelper.HashPassword("Admin123456"),
                            FirstName = "Admin",
                            LastName = "Dos",
                            Status = UserStatus.Active,
                            IsActive = true,
                            CreatedAtUtc = DateTimeOffset.UtcNow,
                            UpdatedAtUtc = DateTimeOffset.UtcNow
                        }
                    };

                    context.Users.AddRange(users);
                    await context.SaveChangesAsync();
                    logger.LogInformation("Usuarios semilla agregados");
                }

                // Seed BusinessProfiles si no existen
                if (!await context.BusinessProfiles.AnyAsync())
                {
                    var profiles = new List<BusinessProfile>
                    {
                        new()
                        {
                            Id = SeedIds.BusinessProfileId1,
                            Name = "Barbería Los Amigos",
                            TradeName = "LosAmigos",
                            TimeZone = "America/Bogota",
                            ContactEmail = "info@losamigos.com",
                            IsActive = true,
                            CreatedAtUtc = DateTimeOffset.UtcNow,
                            UpdatedAtUtc = DateTimeOffset.UtcNow
                        },
                        new()
                        {
                            Id = SeedIds.BusinessProfileId2,
                            Name = "Consultorio Dr. García",
                            TradeName = "DrGarcia",
                            TimeZone = "America/Bogota",
                            ContactEmail = "info@drgarcia.com",
                            IsActive = true,
                            CreatedAtUtc = DateTimeOffset.UtcNow,
                            UpdatedAtUtc = DateTimeOffset.UtcNow
                        }
                    };

                    context.BusinessProfiles.AddRange(profiles);
                    await context.SaveChangesAsync();
                    logger.LogInformation("BusinessProfiles semilla agregados");
                }

                // Seed UserRoles si no existen
                if (!await context.UserRoles.AnyAsync())
                {
                    var userRoles = new List<UserRole>
                    {
                        new()
                        {
                            UserId = SeedIds.AdminUserId1,
                            RoleId = SeedIds.AdminRoleId,
                            AssignedAtUtc = DateTimeOffset.UtcNow
                        },
                        new()
                        {
                            UserId = SeedIds.AdminUserId2,
                            RoleId = SeedIds.AdminRoleId,
                            AssignedAtUtc = DateTimeOffset.UtcNow
                        }
                    };

                    context.UserRoles.AddRange(userRoles);
                    await context.SaveChangesAsync();
                    logger.LogInformation("UserRoles semilla agregados");
                }

                // Seed UserTenants si no existen
                if (!await context.UserTenants.AnyAsync())
                {
                    var userTenants = new List<UserTenant>
                    {
                        // AdminUser1 es propietario de BusinessProfile1 y tiene acceso a BusinessProfile2
                        new()
                        {
                            UserId = SeedIds.AdminUserId1,
                            BusinessProfileId = SeedIds.BusinessProfileId1,
                            IsOwner = true,
                            AssignedAtUtc = DateTimeOffset.UtcNow
                        },
                        new()
                        {
                            UserId = SeedIds.AdminUserId1,
                            BusinessProfileId = SeedIds.BusinessProfileId2,
                            IsOwner = false,
                            AssignedAtUtc = DateTimeOffset.UtcNow
                        },
                        // AdminUser2 es propietario de BusinessProfile2
                        new()
                        {
                            UserId = SeedIds.AdminUserId2,
                            BusinessProfileId = SeedIds.BusinessProfileId2,
                            IsOwner = true,
                            AssignedAtUtc = DateTimeOffset.UtcNow
                        }
                    };

                    context.UserTenants.AddRange(userTenants);
                    await context.SaveChangesAsync();
                    logger.LogInformation("UserTenants semilla agregados");
                }

                logger.LogInformation("Datos semilla de autenticación validados correctamente");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error al agregar datos semilla de autenticación");
                throw;
            }
        }
    }
}
