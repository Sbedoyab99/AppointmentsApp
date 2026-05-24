using AppointmentsApp.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentsApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GenericController(IGenericService service) : BaseController
    {
        private readonly IGenericService _service = service;

        [HttpGet]
        public async Task<IActionResult> Get() => FromAction(await _service.GetEntity());
    }
}
