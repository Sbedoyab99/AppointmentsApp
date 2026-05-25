namespace AppointmentsApp.Domain.Entities
{
    public class UserTenant
    {
        public Guid UserId { get; set; }
        public Guid BusinessProfileId { get; set; }
        public bool IsOwner { get; set; }
        public DateTimeOffset AssignedAtUtc { get; set; }

        public AppUser User { get; set; } = null!;
        public BusinessProfile BusinessProfile { get; set; } = null!;
    }
}
