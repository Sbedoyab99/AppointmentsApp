namespace AppointmentsApp.Application.Helpers
{
    using BCrypt.Net;

    /// <summary>
    /// Helper estático para operaciones de hashing de contraseñas usando BCrypt.
    /// No requiere instanciación.
    /// </summary>
    public static class PasswordHashHelper
    {
        /// <summary>
        /// Hashea una contraseña en texto plano usando BCrypt.
        /// </summary>
        /// <param name="password">Contraseña sin encriptar</param>
        /// <returns>Hash seguro de la contraseña</returns>
        /// <exception cref="ArgumentException">Lanzada si password es null o vacía</exception>
        public static string HashPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("La contraseña no puede estar vacía.", nameof(password));
            }

            return BCrypt.HashPassword(password, workFactor: 12);
        }

        /// <summary>
        /// Verifica si una contraseña coincide con su hash almacenado.
        /// </summary>
        /// <param name="hashedPassword">Hash almacenado en BD</param>
        /// <param name="providedPassword">Contraseña proporcionada por usuario</param>
        /// <returns>true si coinciden, false en caso contrario</returns>
        public static bool VerifyPassword(string hashedPassword, string providedPassword)
        {
            if (string.IsNullOrWhiteSpace(hashedPassword) || string.IsNullOrWhiteSpace(providedPassword))
            {
                return false;
            }

            try
            {
                return BCrypt.Verify(providedPassword, hashedPassword);
            }
            catch
            {
                return false;
            }
        }
    }
}
