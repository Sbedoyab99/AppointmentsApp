using AppointmentsApp.Api.Middlewares;
using AppointmentsApp.Domain.Responses;
using AppointmentsApp.Infrastructure.Data;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using AppointmentsApp.Application.Interfaces;
using AppointmentsApp.Infrastructure.Services;
using Scalar.AspNetCore;
using System.Text.Json;
using Serilog;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi;

namespace AppointmentsApp.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {SourceContext} - {Message:lj}{NewLine}{Exception}")
                .WriteTo.File(
                    path: "logs/log-.txt",
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 30,
                    outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {SourceContext} - {Message:lj}{NewLine}{Exception}"
                )
                .CreateLogger();

            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            builder.Host.UseSerilog();

            var corsOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? [];
            var configuredApiKey = builder.Configuration["ApiKey"] ?? string.Empty;

            // Db Conection
            builder.Services.AddDbContext<DataContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Add middlewares
            builder.Services.AddScoped<ApiKeyValidatorMiddleware>();

            // Add services to the container.
            builder.Services.AddScoped<IGenericService, GenericService>();
            builder.Services.AddScoped<IAuthService, AuthService>();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("DefaultCorsPolicy", policyBuilder =>
                {
                    if (corsOrigins.Length > 0)
                    {
                        policyBuilder.WithOrigins(corsOrigins)
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                        return;
                    }

                    policyBuilder.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                })
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.InvalidModelStateResponseFactory = context =>
                    {
                        var errors = context.ModelState
                            .Where(e => e.Value?.Errors.Count > 0)
                            .SelectMany(kvp => kvp.Value!.Errors.Select(e => $"{kvp.Key}: {e.ErrorMessage}"))
                            .ToList();

                        var response = new ApiResponse
                        {
                            StatusCode = StatusCodes.Status400BadRequest,
                            Message = string.Join(" | ", errors)
                        };

                        return new BadRequestObjectResult(response);
                    };
                });

            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddOpenApi(options =>
            {
                options.AddDocumentTransformer((document, context, cancellationToken) =>
                {
                    document.Components ??= new OpenApiComponents();
                    document.Components.SecuritySchemes ??= new Dictionary<string, IOpenApiSecurityScheme>();
                    document.Components.SecuritySchemes["ApiKey"] = new OpenApiSecurityScheme
                    {
                        Type = SecuritySchemeType.ApiKey,
                        Name = "x-api-key",
                        In = ParameterLocation.Header,
                        Description = "Ingrese su clave de API para acceder a los endpoints protegidos."
                    };
                    return Task.CompletedTask;
                });
            });

            WebApplication app = builder.Build();

            app.UseSerilogRequestLogging();
            Log.Information("Backend Template API started in {Environment}", app.Environment.EnvironmentName);
            app.Lifetime.ApplicationStarted.Register(() =>
            {
                var listeningUrls = string.Join(", ", app.Urls);
                Log.Information("Backend Template API listening on: {Urls}", listeningUrls);
            });

            static async Task WriteApiResponseAsync(HttpContext context, int statusCode, string message)
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
                {
                    return;
                }

                var statusCode = context.Response.StatusCode;

                if (statusCode is not (StatusCodes.Status404NotFound or StatusCodes.Status405MethodNotAllowed))
                {
                    return;
                }

                var message = statusCode switch
                {
                    StatusCodes.Status404NotFound => "La ruta solicitada no existe.",
                    StatusCodes.Status405MethodNotAllowed => "El método HTTP no está permitido para esta ruta.",
                    _ => "La solicitud no pudo ser procesada."
                };

                await WriteApiResponseAsync(context, statusCode, message);
            });

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
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
            }

            app.UseHttpsRedirection();
            app.UseCors("DefaultCorsPolicy");
            app.UseMiddleware<ApiKeyValidatorMiddleware>();
            app.UseAuthorization();

            app.MapControllers();

            try
            {
                Log.Information("Starting web host");
                app.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}
