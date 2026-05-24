using AppointmentsApp.Application.Interfaces;
using AppointmentsApp.Domain.Entities;
using AppointmentsApp.Domain.Enums;
using AppointmentsApp.Domain.Responses;

namespace AppointmentsApp.Application.Services
{
    /// <summary>
    /// Los servicios son donde se ejecuta la logica del negocio.
    /// </summary>
    public class GenericService : IGenericService
    {
        public Task<ActionResponse<Entity>> GetEntity()
        {
            Entity entity = new()
            {
                Id = 1,
                CreatedAt = DateTime.Now,
                Message = "Has ejecutado una operacion!",
                State = EntityState.created
            };

            return Task.FromResult(ActionResponse<Entity>.Ok(entity, "Has ejecutado una operacion!"));
        }
    }
}
