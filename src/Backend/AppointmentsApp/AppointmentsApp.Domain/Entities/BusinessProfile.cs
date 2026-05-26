namespace AppointmentsApp.Domain.Entities
{
    public class BusinessProfile
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string TradeName { get; set; } = string.Empty;
        public string NormalizedTradeName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string TimeZone { get; set; } = "America/Bogota";
        public string ContactEmail { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public DateTimeOffset CreatedAtUtc { get; set; }
        public DateTimeOffset UpdatedAtUtc { get; set; }

        public ICollection<UserTenant> UserTenants { get; set; } = [];
    }
}
