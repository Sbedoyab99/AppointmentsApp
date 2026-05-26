using AppointmentsApp.Application.Interfaces;
using AppointmentsApp.Domain.DTOs.BusinessProfile;
using AppointmentsApp.Domain.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentsApp.Api.Controllers
{
    [ApiController]
    [Route("api/admin/business-profile")]
    [Authorize(Policy = "AdminOnly")]
    public class BusinessProfileController(IBusinessProfileService businessProfileService, ILogger<BusinessProfileController> logger) : BaseController
    {
        private readonly IBusinessProfileService _businessProfileService = businessProfileService;
        private readonly ILogger<BusinessProfileController> _logger = logger;

        /// <summary>
        /// Obtiene el perfil del negocio actual.
        /// </summary>
        /// <param name="cancellationToken">Token de cancelación</param>
        /// <returns>Datos del perfil del negocio</returns>
        /// <response code="200">Perfil obtenido exitosamente</response>
        /// <response code="401">Token no proporcionado o inválido</response>
        /// <response code="403">Usuario sin rol Admin</response>
        /// <response code="404">Perfil del negocio no encontrado</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            if (!TryGetTenantIdFromClaims(out Guid businessProfileId))
            {
                _logger.LogWarning("TenantId no encontrado en los claims del JWT");
                return Unauthorized("Token inválido o TenantId no encontrado en sesión");
            }

            _logger.LogInformation("Obteniendo perfil del negocio {ProfileId}", businessProfileId);
            ActionResponse<BusinessProfileResponse> response = await _businessProfileService.GetAsync(businessProfileId, cancellationToken);
            return FromAction(response);
        }

        /// <summary>
        /// Actualiza la información del perfil del negocio.
        /// </summary>
        /// <param name="request">Datos actualizados del perfil</param>
        /// <param name="cancellationToken">Token de cancelación</param>
        /// <returns>Perfil actualizado</returns>
        /// <response code="200">Perfil actualizado exitosamente</response>
        /// <response code="400">Datos de entrada inválidos</response>
        /// <response code="401">Token no proporcionado o inválido</response>
        /// <response code="403">Usuario sin rol Admin</response>
        /// <response code="404">Perfil del negocio no encontrado</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(
            [FromBody] UpdateBusinessProfileRequest request,
            CancellationToken cancellationToken)
        {
            if (!TryGetTenantIdFromClaims(out Guid businessProfileId))
            {
                _logger.LogWarning("TenantId no encontrado en los claims del JWT");
                return Unauthorized("Token inválido o TenantId no encontrado en sesión");
            }

            _logger.LogInformation("Actualizando perfil del negocio {ProfileId}", businessProfileId);
            ActionResponse<BusinessProfileResponse> response = await _businessProfileService.UpdateAsync(businessProfileId, request, cancellationToken);
            return FromAction(response);
        }
    }
}
