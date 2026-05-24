using AppointmentsApp.Domain.DTOs;
using AppointmentsApp.Domain.Entities;

namespace AppointmentsApp.Application.Mappers
{
    public static class EntityMapper
    {
        public static EntityDto ToEntityDto(Entity entity)
        {
            return new EntityDto
            {
                Message = entity.Message
            };
        }
    }
}
