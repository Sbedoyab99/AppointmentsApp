using AppointmentsApp.Api.Extensions;
using AppointmentsApp.Api.Middlewares;
using Serilog;

namespace AppointmentsApp.Api;

public class Program
{
    public static async Task Main(string[] args)
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

        builder.Services
            .AddDatabase(builder.Configuration)
            .AddApplicationServices()
            .AddJwtAuthentication(builder.Configuration)
            .AddAuthorizationPolicies()
            .AddCorsPolicy(builder.Configuration)
            .AddApiServices()
            .AddOpenApiDocumentation();

        WebApplication app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            await app.UseDevelopmentDatabaseAsync();
            app.UseOpenApiDocumentation(builder.Configuration["ApiKey"] ?? string.Empty);
        }

        app.UseSerilogRequestLogging();
        Log.Information("Backend Template API started in {Environment}", app.Environment.EnvironmentName);
        app.Lifetime.ApplicationStarted.Register(() =>
        {
            var listeningUrls = string.Join(", ", app.Urls);
            Log.Information("Backend Template API listening on: {Urls}", listeningUrls);
        });

        app.UseExceptionHandling();
        app.UseHttpsRedirection();
        app.UseCors("DefaultCorsPolicy");
        app.UseMiddleware<ApiKeyValidatorMiddleware>();
        app.UseAuthentication();
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
