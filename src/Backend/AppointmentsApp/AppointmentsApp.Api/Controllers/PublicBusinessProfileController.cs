using AppointmentsApp.Application.Interfaces;
using AppointmentsApp.Domain.DTOs.BusinessProfile;
using AppointmentsApp.Domain.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentsApp.Api.Controllers
{
    [ApiController]
    [Route("api/public/business-profile")]
    [AllowAnonymous]
    public class PublicBusinessProfileController(IBusinessProfileService businessProfileService, ILogger<PublicBusinessProfileController> logger) : BaseController
    {
        private readonly IBusinessProfileService _businessProfileService = businessProfileService;
        private readonly ILogger<PublicBusinessProfileController> _logger = logger;

        /// <summary>
        /// Obtiene el perfil del negocio (endpoint público sin autenticación).
        /// </summary>
        /// <param name="businessProfileId">ID del negocio</param>
        /// <param name="cancellationToken">Token de cancelación</param>
        /// <returns>Datos del perfil del negocio</returns>
        /// <response code="200">Perfil obtenido exitosamente</response>
        /// <response code="400">BusinessProfileId requerido y válido</response>
        /// <response code="404">Perfil del negocio no encontrado o inactivo</response>
        /// <response code="429">Too Many Requests - rate limit excedido</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get([FromQuery] Guid businessProfileId, CancellationToken cancellationToken)
        {
            if (businessProfileId == Guid.Empty)
            {
                _logger.LogWarning("BusinessProfileId no proporcionado o inválido en endpoint público");
                return FromAction(ActionResponse<BusinessProfileResponse>.BadRequest("BusinessProfileId es requerido"));
            }

            _logger.LogInformation("Obteniendo perfil del negocio {ProfileId} (endpoint público)", businessProfileId);
            ActionResponse<BusinessProfileResponse> response = await _businessProfileService.GetAsync(businessProfileId, cancellationToken);
            return FromAction(response);
        }
    }
}
