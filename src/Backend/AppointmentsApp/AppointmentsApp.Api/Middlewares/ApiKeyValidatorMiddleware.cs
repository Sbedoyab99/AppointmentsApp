using AppointmentsApp.Domain.Responses;
using Microsoft.Extensions.Primitives;
using System.Text.Json;

namespace AppointmentsApp.Api.Middlewares
{
    /// <summary>
    /// Clase para validar el Api Key configurado en el appsettings.
    /// </summary>
    /// <param name="next"></param>
    public class ApiKeyValidatorMiddleware(IConfiguration configuration) : IMiddleware
    {
        private readonly IConfiguration _config = configuration;

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (context.Request.Path.StartsWithSegments("/scalar") || context.Request.Path.StartsWithSegments("/openapi"))
            {
                await next(context);
                return;
            }

            context.Response.ContentType = "application/json";

            if (!context.Request.Headers.TryGetValue("x-api-key", out StringValues extractedApiKey))
            {
                var response = new ApiResponse
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "El header x-api-key es obligatorio."
                };

                context.Response.StatusCode = response.StatusCode;
                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
                return;
            }

            if (!string.Equals(extractedApiKey, _config["ApiKey"], StringComparison.Ordinal))
            {
                var response = new ApiResponse
                {
                    StatusCode = StatusCodes.Status401Unauthorized,
                    Message = "La clave x-api-key proporcionada no es válida."
                };

                context.Response.StatusCode = response.StatusCode;
                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
                return;
            }

            await next(context);
        }
    }
}
