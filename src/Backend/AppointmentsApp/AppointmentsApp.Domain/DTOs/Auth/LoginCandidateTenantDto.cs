namespace AppointmentsApp.Domain.DTOs.Auth
{
    public class LoginCandidateTenantDto
    {
        public Guid TenantId { get; set; }
        public string TenantName { get; set; } = string.Empty;
        public bool IsOwner { get; set; }
    }
}
