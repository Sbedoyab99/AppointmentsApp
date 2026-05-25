namespace AppointmentsApp.Application.Validators
{
    public static class LoginRequestValidator
    {
        public static (bool IsValid, string? Error) Validate(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return (false, "El email es requerido");
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                return (false, "La contraseña es requerida");
            }

            if (email.Length > 255)
            {
                return (false, "El email no puede exceder 255 caracteres");
            }

            if (!IsValidEmail(email))
            {
                return (false, "El formato del email es inválido");
            }

            if (password.Length < 8)
            {
                return (false, "La contraseña debe tener al menos 8 caracteres");
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
    }
}
