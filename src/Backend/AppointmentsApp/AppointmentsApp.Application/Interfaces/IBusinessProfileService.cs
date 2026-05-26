namespace AppointmentsApp.Application.Interfaces
{
    using AppointmentsApp.Domain.DTOs.BusinessProfile;
    using AppointmentsApp.Domain.Responses;

    /// <summary>
    /// Servicio para gestionar el perfil del negocio.
    /// </summary>
    public interface IBusinessProfileService
    {
        /// <summary>
        /// Obtiene el perfil del negocio por su ID.
        /// </summary>
        Task<ActionResponse<BusinessProfileResponse>> GetAsync(Guid businessProfileId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Actualiza los datos del perfil del negocio.
        /// </summary>
        Task<ActionResponse<BusinessProfileResponse>> UpdateAsync(
            Guid businessProfileId,
            UpdateBusinessProfileRequest request,
            CancellationToken cancellationToken = default);
    }
}
