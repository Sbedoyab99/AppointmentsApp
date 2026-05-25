namespace AppointmentsApp.Application.Interfaces
{
    using AppointmentsApp.Domain.DTOs.Auth;
    using AppointmentsApp.Domain.Responses;

    /// <summary>
    /// Servicio de autenticación para validar credenciales y crear sesiones.
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Valida credenciales de usuario y retorna tenants disponibles.
        /// </summary>
        Task<ActionResponse<LoginResponseDto>> ValidateCredentialsAsync(
            LoginRequestDto request,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Crea sesión con access token y refresh token para un tenant específico.
        /// </summary>
        Task<ActionResponse<AuthSessionResponseDto>> CreateTenantSessionAsync(
            SelectTenantRequestDto request,
            CancellationToken cancellationToken = default);
    }
}
