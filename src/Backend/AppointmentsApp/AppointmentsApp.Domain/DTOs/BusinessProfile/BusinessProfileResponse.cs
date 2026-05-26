namespace AppointmentsApp.Domain.DTOs.BusinessProfile
{
    public class BusinessProfileResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string TradeName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string TimeZone { get; set; } = null!;
        public string ContactEmail { get; set; } = string.Empty;
    }
}
