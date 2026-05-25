namespace AppointmentsApp.Domain.DTOs.Auth
{
    public class SelectTenantRequestDto
    {
        public Guid UserId { get; set; }
        public Guid TenantId { get; set; }
    }
}
