using AppointmentsApp.Application.Interfaces;
using AppointmentsApp.Application.Validators;
using AppointmentsApp.Domain.DTOs.Auth;
using AppointmentsApp.Domain.Responses;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentsApp.Api.Controllers
{
    [ApiController]
    [Route("api/public/auth")]
    public class AuthController(IAuthService authService, ILogger<AuthController> logger) : BaseController
    {
        private readonly IAuthService _authService = authService;
        private readonly ILogger<AuthController> _logger = logger;

        /// <summary>
        /// Valida credenciales del usuario y retorna lista de tenants disponibles
        /// </summary>
        /// <param name="request">Credenciales de usuario (email y contraseña)</param>
        /// <param name="cancellationToken">Token de cancelación</param>
        /// <returns>LoginResponseDto con lista de tenants o mensaje de error</returns>
        /// <response code="200">Credenciales validadas exitosamente</response>
        /// <response code="400">Email o contraseña vacío, o formato inválido</response>
        /// <response code="401">Credenciales inválidas o usuario desactivado</response>
        /// <response code="403">Usuario desactivado o sin acceso a tenants</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login(
            [FromBody] LoginRequestDto request,
            CancellationToken cancellationToken)
        {
            (var isValid, var error) = LoginRequestValidator.Validate(request.Email, request.Password);
            if (!isValid)
            {
                _logger.LogWarning("Validación fallida en login: {Error}", error);
                return FromAction(ActionResponse<LoginResponseDto>.BadRequest(error ?? "Datos inválidos"));
            }

            _logger.LogInformation("Login attempt for email: {Email}", request.Email);
            ActionResponse<LoginResponseDto> response = await _authService.ValidateCredentialsAsync(request, cancellationToken);
            return FromAction(response);
        }

        /// <summary>
        /// Selecciona un tenant y crea sesión con JWT
        /// </summary>
        /// <param name="request">Datos del usuario y tenant a seleccionar</param>
        /// <param name="cancellationToken">Token de cancelación</param>
        /// <returns>AuthSessionResponseDto con AccessToken y RefreshToken</returns>
        /// <response code="200">Sesión creada exitosamente con JWT</response>
        /// <response code="400">UserId o TenantId vacío</response>
        /// <response code="403">Usuario sin permiso en el tenant o tenant inactivo</response>
        /// <response code="404">Usuario o tenant no encontrado</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpPost("select-tenant")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SelectTenant(
            [FromBody] SelectTenantRequestDto request,
            CancellationToken cancellationToken)
        {
            (var isValid, var error) = SelectTenantRequestValidator.Validate(request.UserId, request.TenantId);
            if (!isValid)
            {
                _logger.LogWarning("Validación fallida en select-tenant: {Error}", error);
                return FromAction(ActionResponse<AuthSessionResponseDto>.BadRequest(error ?? "Datos inválidos"));
            }

            _logger.LogInformation("Tenant selection for user: {UserId}", request.UserId);
            ActionResponse<AuthSessionResponseDto> response = await _authService.CreateTenantSessionAsync(request, cancellationToken);
            return FromAction(response);
        }
    }
}
