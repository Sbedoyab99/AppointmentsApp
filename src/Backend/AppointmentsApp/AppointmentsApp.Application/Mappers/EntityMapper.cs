using AppointmentsApp.Domain.DTOs;
using AppointmentsApp.Domain.DTOs.BusinessProfile;
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

        /// <summary>
        /// Mapea una entidad BusinessProfile a BusinessProfileResponse.
        /// </summary>
        public static BusinessProfileResponse ToBusinessProfileResponse(BusinessProfile profile)
        {
            return new BusinessProfileResponse
            {
                Id = profile.Id,
                Name = profile.Name,
                TradeName = profile.TradeName,
                Description = profile.Description,
                Phone = profile.Phone,
                Address = profile.Address,
                TimeZone = profile.TimeZone,
                ContactEmail = profile.ContactEmail
            };
        }
    }
}
