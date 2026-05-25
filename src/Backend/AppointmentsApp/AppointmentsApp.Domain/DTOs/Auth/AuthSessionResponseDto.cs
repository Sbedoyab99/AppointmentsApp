namespace AppointmentsApp.Domain.DTOs.Auth
{
    public class AuthSessionResponseDto
    {
        public string AccessToken { get; set; } = string.Empty;
        public DateTimeOffset ExpiresAtUtc { get; set; }
        public string? RefreshToken { get; set; }
        public Guid TenantId { get; set; }
    }
}
