using AppointmentsApp.Domain.Responses;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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

        /// <summary>
        /// Intenta extraer el TenantId (BusinessProfileId) desde los claims del JWT.
        /// </summary>
        /// <param name="tenantId">El TenantId extraído, Guid.Empty si falla.</param>
        /// <returns>True si se extrajo correctamente, False en caso contrario.</returns>
        protected bool TryGetTenantIdFromClaims(out Guid tenantId)
        {
            tenantId = Guid.Empty;
            Claim? tenantIdClaim = User.FindFirst("tenantId");

            if (tenantIdClaim is null)
            {
                return false;
            }

            if (Guid.TryParse(tenantIdClaim.Value, out Guid parsedTenantId))
            {
                tenantId = parsedTenantId;
                return true;
            }

            return false;
        }
    }
}
