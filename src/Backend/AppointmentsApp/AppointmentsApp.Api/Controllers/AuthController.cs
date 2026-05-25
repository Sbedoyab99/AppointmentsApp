using AppointmentsApp.Application.Interfaces;
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

        [HttpPost("login")]
        public async Task<IActionResult> Login(
            [FromBody] LoginRequestDto request,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation("Login attempt for email: {Email}", request.Email);
            ActionResponse<LoginResponseDto> response = await _authService.ValidateCredentialsAsync(request, cancellationToken);
            return FromAction(response);
        }

        [HttpPost("select-tenant")]
        public async Task<IActionResult> SelectTenant(
            [FromBody] SelectTenantRequestDto request,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation("Tenant selection for user: {UserId}", request.UserId);
            ActionResponse<AuthSessionResponseDto> response = await _authService.CreateTenantSessionAsync(request, cancellationToken);
            return FromAction(response);
        }
    }
}
