using AppointmentsApp.Domain.Entities;
using AppointmentsApp.Domain.Responses;

namespace AppointmentsApp.Application.Interfaces
{
    public interface IGenericService
    {
        Task<ActionResponse<Entity>> GetEntity();
    }
}
