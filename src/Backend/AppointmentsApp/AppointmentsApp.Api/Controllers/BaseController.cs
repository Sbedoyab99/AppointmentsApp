using AppointmentsApp.Domain.Responses;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentsApp.Api.Controllers
{
    public abstract class BaseController : ControllerBase
    {
        protected IActionResult FromAction<T>(ActionResponse<T> response)
        {
            if (response.WasSuccess && response.Result is not null)
            {
                return StatusCode(response.StatusCode, new ApiResponseData<T>
                {
                    StatusCode = response.StatusCode,
                    Message = response.Message,
                    Data = response.Result
                });
            }

            return StatusCode(response.StatusCode, new ApiResponse
            {
                StatusCode = response.StatusCode,
                Message = response.Message
            });
        }
    }
}
