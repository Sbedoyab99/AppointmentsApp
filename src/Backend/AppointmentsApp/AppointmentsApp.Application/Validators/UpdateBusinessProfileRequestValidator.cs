namespace AppointmentsApp.Application.Validators
{
    public static class UpdateBusinessProfileRequestValidator
    {
        public static (bool IsValid, string? Error) Validate(
            string? name,
            string? tradeName,
            string? description,
            string? phone,
            string? address,
            string? contactEmail,
            string? timeZone)
        {
            // Validar Name
            if (string.IsNullOrWhiteSpace(name))
            {
                return (false, "El nombre del negocio es obligatorio.");
            }

            if (name.Length < 2 || name.Length > 200)
            {
                return (false, "El nombre debe tener entre 2 y 200 caracteres.");
            }

            // Validar TradeName
            if (string.IsNullOrWhiteSpace(tradeName))
            {
                return (false, "El nombre comercial es obligatorio.");
            }

            if (tradeName.Length < 2 || tradeName.Length > 150)
            {
                return (false, "El nombre comercial debe tener entre 2 y 150 caracteres.");
            }

            // Validar Description (opcional)
            if (!string.IsNullOrWhiteSpace(description) && description.Length > 1000)
            {
                return (false, "La descripción no puede superar 1000 caracteres.");
            }

            // Validar Phone (opcional)
            if (!string.IsNullOrWhiteSpace(phone) && phone.Length > 30)
            {
                return (false, "El teléfono no puede superar 30 caracteres.");
            }

            // Validar Address (opcional)
            if (!string.IsNullOrWhiteSpace(address) && address.Length > 300)
            {
                return (false, "La dirección no puede superar 300 caracteres.");
            }

            // Validar ContactEmail (opcional pero si se proporciona debe ser válido)
            if (!string.IsNullOrWhiteSpace(contactEmail))
            {
                if (contactEmail.Length > 320)
                {
                    return (false, "El correo electrónico no puede superar 320 caracteres.");
                }

                if (!IsValidEmail(contactEmail))
                {
                    return (false, "El correo electrónico no tiene un formato válido.");
                }
            }

            // Validar TimeZone
            if (string.IsNullOrWhiteSpace(timeZone))
            {
                return (false, "La zona horaria es obligatoria.");
            }

            if (!IsValidTimeZone(timeZone))
            {
                return (false, $"La zona horaria '{timeZone}' no es válida. Use una zona horaria IANA como 'America/Bogota'.");
            }

            return (true, null);
        }

        private static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private static bool IsValidTimeZone(string timeZoneId)
        {
            try
            {
                TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
