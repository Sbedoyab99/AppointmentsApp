namespace AppointmentsApp.Domain.DTOs.Auth
{
    public class LoginResponseDto
    {
        public Guid UserId { get; set; }
        public bool RequiresTenantSelection { get; set; }
        public List<LoginCandidateTenantDto> Tenants { get; set; } = [];
        public string Message { get; set; } = string.Empty;
    }
}
