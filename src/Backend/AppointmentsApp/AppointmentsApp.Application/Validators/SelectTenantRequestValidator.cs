namespace AppointmentsApp.Application.Validators
{
    public static class SelectTenantRequestValidator
    {
        public static (bool IsValid, string? Error) Validate(Guid userId, Guid tenantId)
        {
            if (userId == Guid.Empty)
            {
                return (false, "El ID del usuario es requerido y no puede estar vacío");
            }

            if (tenantId == Guid.Empty)
            {
                return (false, "El ID del negocio es requerido y no puede estar vacío");
            }

            return (true, null);
        }
    }
}
