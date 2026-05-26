using AppointmentsApp.Application.Interfaces;
using AppointmentsApp.Application.Mappers;
using AppointmentsApp.Application.Validators;
using AppointmentsApp.Domain.DTOs.BusinessProfile;
using AppointmentsApp.Domain.Entities;
using AppointmentsApp.Domain.Responses;
using AppointmentsApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AppointmentsApp.Infrastructure.Services
{
    public class BusinessProfileService(DataContext context, ILogger<BusinessProfileService> logger) : IBusinessProfileService
    {
        private readonly DataContext _context = context;
        private readonly ILogger<BusinessProfileService> _logger = logger;

        public async Task<ActionResponse<BusinessProfileResponse>> GetAsync(Guid businessProfileId, CancellationToken cancellationToken = default)
        {
            try
            {
                BusinessProfile? profile = await _context.BusinessProfiles
                    .FirstOrDefaultAsync(b => b.Id == businessProfileId && b.IsActive, cancellationToken);

                if (profile is null)
                {
                    _logger.LogWarning("Perfil del negocio con ID {ProfileId} no encontrado o inactivo", businessProfileId);
                    return ActionResponse<BusinessProfileResponse>.NotFound("Perfil del negocio no encontrado.");
                }

                var response = EntityMapper.ToBusinessProfileResponse(profile);
                return ActionResponse<BusinessProfileResponse>.Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener perfil del negocio con ID {ProfileId}", businessProfileId);
                return ActionResponse<BusinessProfileResponse>.InternalServerError("Error al obtener el perfil del negocio");
            }
        }

        public async Task<ActionResponse<BusinessProfileResponse>> UpdateAsync(
            Guid businessProfileId,
            UpdateBusinessProfileRequest request,
            CancellationToken cancellationToken = default)
        {
            try
            {
                // Validar los datos de entrada
                var (isValid, validationError) = UpdateBusinessProfileRequestValidator.Validate(
                    request.Name,
                    request.TradeName,
                    request.Description,
                    request.Phone,
                    request.Address,
                    request.ContactEmail,
                    request.TimeZone);

                if (!isValid)
                {
                    _logger.LogWarning("Error de validación en UpdateBusinessProfile: {Error}", validationError);
                    return ActionResponse<BusinessProfileResponse>.BadRequest(validationError!);
                }

                BusinessProfile? profile = await _context.BusinessProfiles
                    .FirstOrDefaultAsync(b => b.Id == businessProfileId && b.IsActive, cancellationToken);

                if (profile is null)
                {
                    _logger.LogWarning("Intento de actualizar perfil con ID {ProfileId} no encontrado o inactivo", businessProfileId);
                    return ActionResponse<BusinessProfileResponse>.NotFound("Perfil del negocio no encontrado.");
                }

                // Actualizar los campos
                profile.Name = request.Name;
                profile.TradeName = request.TradeName;
                profile.Description = request.Description;
                profile.Phone = request.Phone;
                profile.Address = request.Address;
                profile.TimeZone = request.TimeZone;
                profile.ContactEmail = request.ContactEmail;
                profile.UpdatedAtUtc = DateTimeOffset.UtcNow;

                _context.BusinessProfiles.Update(profile);
                await _context.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("Perfil del negocio con ID {ProfileId} actualizado correctamente", businessProfileId);

                var response = EntityMapper.ToBusinessProfileResponse(profile);
                return ActionResponse<BusinessProfileResponse>.Ok(response, "Perfil del negocio actualizado correctamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar perfil del negocio con ID {ProfileId}", businessProfileId);
                return ActionResponse<BusinessProfileResponse>.InternalServerError("Error al actualizar el perfil del negocio");
            }
        }
    }
}
