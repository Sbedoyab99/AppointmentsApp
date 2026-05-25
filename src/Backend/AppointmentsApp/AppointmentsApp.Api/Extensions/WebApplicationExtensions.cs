using AppointmentsApp.Domain.Responses;
using AppointmentsApp.Infrastructure.Data;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using Serilog;
using System.Text.Json;

namespace AppointmentsApp.Api.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplication UseExceptionHandling(this WebApplication app)
    {
        app.UseExceptionHandler(errorApp =>
        {
            errorApp.Run(async context =>
            {
                IExceptionHandlerPathFeature? exceptionFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                Log.Error(exceptionFeature?.Error, "Unhandled exception for {Path}", exceptionFeature?.Path ?? context.Request.Path.Value);
                await WriteApiResponseAsync(context, StatusCodes.Status500InternalServerError, "Ocurrió un error inesperado.");
            });
        });

        app.UseStatusCodePages(async statusCodeContext =>
        {
            HttpContext context = statusCodeContext.HttpContext;

            if (context.Response.HasStarted)
                return;

            var statusCode = context.Response.StatusCode;

            if (statusCode is not (StatusCodes.Status404NotFound or StatusCodes.Status405MethodNotAllowed))
                return;

            var message = statusCode switch
            {
                StatusCodes.Status404NotFound => "La ruta solicitada no existe.",
                StatusCodes.Status405MethodNotAllowed => "El método HTTP no está permitido para esta ruta.",
                _ => "La solicitud no pudo ser procesada."
            };

            await WriteApiResponseAsync(context, statusCode, message);
        });

        return app;
    }

    public static async Task UseDevelopmentDatabaseAsync(this WebApplication app)
    {
        using IServiceScope scope = app.Services.CreateScope();
        DataContext context = scope.ServiceProvider.GetRequiredService<DataContext>();
        ILoggerFactory loggerFactory = scope.ServiceProvider.GetRequiredService<ILoggerFactory>();

        await context.Database.MigrateAsync();

        if (!await context.Users.AnyAsync())
        {
            await AuthDataSeeder.SeedAsync(context, loggerFactory);
        }
    }

    public static WebApplication UseOpenApiDocumentation(this WebApplication app, string configuredApiKey)
    {
        app.MapOpenApi("/openapi/{documentName}.json");

        app.MapScalarApiReference("/scalar", options =>
        {
            options.WithTitle("AppointmentsApp API");
            options.WithTheme(ScalarTheme.DeepSpace);
            options.WithOpenApiRoutePattern("/openapi/{documentName}.json");
            options.AddApiKeyAuthentication("ApiKey", scheme =>
            {
                scheme.Name = "x-api-key";
                scheme.Value = configuredApiKey;
                scheme.Description = "Ingrese su clave de API para acceder a los endpoints protegidos.";
            });
        });

        return app;
    }

    private static async Task WriteApiResponseAsync(HttpContext context, int statusCode, string message)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        var response = new ApiResponse
        {
            StatusCode = statusCode,
            Message = message
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}
