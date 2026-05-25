using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentsApp.Api.Controllers
{
    [ApiController]
    [Route("api/admin")]
    [Authorize(Policy = "AdminOnly")]
    public class AdminController(ILogger<AdminController> logger) : BaseController
    {
        private readonly ILogger<AdminController> _logger = logger;

        /// <summary>
        /// Verifica que solo usuarios con rol Admin pueden acceder
        /// </summary>
        /// <returns>Estado de acceso al panel administrativo</returns>
        /// <response code="200">Acceso concedido al panel administrativo</response>
        /// <response code="401">Token no proporcionado o inválido</response>
        /// <response code="403">Usuario sin rol Admin</response>
        [HttpGet("health")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult Health()
        {
            _logger.LogInformation("Admin health check accessed");
            var response = new { status = "Admin panel is accessible" };
            return Ok(response);
        }
    }
}
