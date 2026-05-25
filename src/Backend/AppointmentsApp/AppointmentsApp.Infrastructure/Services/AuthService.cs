using AppointmentsApp.Application.Helpers;
using AppointmentsApp.Application.Interfaces;
using AppointmentsApp.Domain.DTOs.Auth;
using AppointmentsApp.Domain.Entities;
using AppointmentsApp.Domain.Enums;
using AppointmentsApp.Domain.Responses;
using AppointmentsApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace AppointmentsApp.Infrastructure.Services
{
    public class AuthService(DataContext context, ILogger<AuthService> logger, IConfiguration configuration) : IAuthService
    {
        private readonly DataContext _context = context;
        private readonly ILogger<AuthService> _logger = logger;
        private readonly IConfiguration _configuration = configuration;

        public async Task<ActionResponse<LoginResponseDto>> ValidateCredentialsAsync(
            LoginRequestDto request,
            CancellationToken cancellationToken = default)
        {
            try
            {
                // Buscar usuario por email normalizado
                AppUser? user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email.ToLower() == request.Email.ToLower(), cancellationToken);

                if (user is null)
                {
                    _logger.LogWarning("Intento de login con email inexistente: {Email}", request.Email);
                    return ActionResponse<LoginResponseDto>.Unauthorized("Credenciales inválidas");
                }

                // Verificar que el usuario esté activo
                if (!user.IsActive || user.Status != UserStatus.Active)
                {
                    _logger.LogWarning("Intento de login con usuario inactivo: {Email}", request.Email);
                    return ActionResponse<LoginResponseDto>.Forbidden("Usuario desactivado o bloqueado");
                }

                // Validar contraseña
                if (!PasswordHashHelper.VerifyPassword(user.PasswordHash, request.Password))
                {
                    _logger.LogWarning("Intento de login con contraseña inválida: {Email}", request.Email);
                    return ActionResponse<LoginResponseDto>.Unauthorized("Credenciales inválidas");
                }

                // Obtener tenants activos del usuario
                List<LoginCandidateTenantDto> tenants = await _context.UserTenants
                    .Where(ut => ut.UserId == user.Id)
                    .Include(ut => ut.BusinessProfile)
                    .Where(ut => ut.BusinessProfile!.IsActive)
                    .Select(ut => new LoginCandidateTenantDto
                    {
                        TenantId = ut.BusinessProfileId,
                        TenantName = ut.BusinessProfile!.TradeName,
                        IsOwner = ut.IsOwner
                    })
                    .ToListAsync(cancellationToken);

                if (tenants.Count == 0)
                {
                    _logger.LogWarning("Usuario sin acceso a tenants: {Email}", request.Email);
                    return ActionResponse<LoginResponseDto>.Forbidden(
                        "No tienes acceso a ningún negocio. Contacta administrador");
                }

                var response = new LoginResponseDto
                {
                    RequiresTenantSelection = tenants.Count > 1,
                    Tenants = tenants,
                    Message = "Credenciales validadas. Selecciona un negocio."
                };

                _logger.LogInformation("Login exitoso para {Email} con {TenantCount} tenants",
                    request.Email, tenants.Count);

                return ActionResponse<LoginResponseDto>.Ok(response, "Credenciales validadas.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en ValidateCredentialsAsync");
                return ActionResponse<LoginResponseDto>.InternalServerError("Error al validar credenciales");
            }
        }

        public async Task<ActionResponse<AuthSessionResponseDto>> CreateTenantSessionAsync(
            SelectTenantRequestDto request,
            CancellationToken cancellationToken = default)
        {
            try
            {
                // Validar que UserTenant existe
                UserTenant? userTenant = await _context.UserTenants
                    .Include(ut => ut.BusinessProfile)
                    .FirstOrDefaultAsync(ut =>
                        ut.UserId == request.UserId &&
                        ut.BusinessProfileId == request.TenantId,
                        cancellationToken);

                if (userTenant is null)
                {
                    _logger.LogWarning("Acceso denegado a tenant {TenantId} para usuario {UserId}",
                        request.TenantId, request.UserId);
                    return ActionResponse<AuthSessionResponseDto>.Forbidden(
                        "No tienes permiso en este negocio");
                }

                // Validar que tenant esté activo
                if (!userTenant.BusinessProfile!.IsActive)
                {
                    _logger.LogWarning("Intento de acceso a tenant inactivo {TenantId}", request.TenantId);
                    return ActionResponse<AuthSessionResponseDto>.Forbidden("Negocio no disponible");
                }

                // Obtener usuario y rol
                AppUser? user = await _context.Users
                    .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                    .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

                if (user is null)
                {
                    return ActionResponse<AuthSessionResponseDto>.NotFound("Usuario no encontrado");
                }

                // Generar Access Token JWT
                var accessToken = GenerateAccessToken(user, userTenant.BusinessProfile);
                DateTimeOffset expiresAtUtc = DateTimeOffset.UtcNow.AddHours(1);

                // Generar y almacenar Refresh Token
                var refreshTokenId = Guid.NewGuid();
                var refreshTokenHash = PasswordHashHelper.HashPassword(refreshTokenId.ToString());

                var refreshToken = new RefreshToken
                {
                    Id = refreshTokenId,
                    UserId = user.Id,
                    TokenHash = refreshTokenHash,
                    ExpiresAtUtc = DateTimeOffset.UtcNow.AddDays(7)
                };

                _context.RefreshTokens.Add(refreshToken);
                await _context.SaveChangesAsync(cancellationToken);

                var response = new AuthSessionResponseDto
                {
                    AccessToken = accessToken,
                    ExpiresAtUtc = expiresAtUtc,
                    RefreshToken = refreshTokenId.ToString(),
                    TenantId = request.TenantId
                };

                _logger.LogInformation("Sesión creada para usuario {Email} en tenant {TenantName}",
                    user.Email, userTenant.BusinessProfile.TradeName);

                return ActionResponse<AuthSessionResponseDto>.Ok(response, "Sesión iniciada correctamente");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en CreateTenantSessionAsync");
                return ActionResponse<AuthSessionResponseDto>.InternalServerError(
                    "Error al crear sesión");
            }
        }

        private string GenerateAccessToken(AppUser user, BusinessProfile tenant)
        {
            var jwtSecret = _configuration["Jwt:Secret"];
            var jwtIssuer = _configuration["Jwt:Issuer"];
            var jwtAudience = _configuration["Jwt:Audience"];

            if (string.IsNullOrEmpty(jwtSecret))
                throw new InvalidOperationException("Jwt:Secret no configurado");

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(jwtSecret);

            var role = user.UserRoles.FirstOrDefault()?.Role.Name ?? "User";

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                [
                    new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new(ClaimTypes.Email, user.Email),
                    new(ClaimTypes.Role, role),
                    new("tenantId", tenant.Id.ToString()),
                    new("tenantName", tenant.TradeName)
                ]),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = jwtIssuer,
                Audience = jwtAudience,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
