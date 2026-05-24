using AppointmentsApp.Domain.Enums;

namespace AppointmentsApp.Domain.Entities
{
    public class Entity
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Message { get; set; } = null!;
        public EntityState State { get; set; }
    }
}
