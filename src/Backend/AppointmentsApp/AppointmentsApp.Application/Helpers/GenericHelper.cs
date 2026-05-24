using System.ComponentModel;

namespace AppointmentsApp.Application.Helpers
{
    /// <summary>
    /// Clase Generica para crear tus helpers, puedes crearlos en esta misma clase o crear clases especializadas para cada helper dependiendo de su funcionalidad.
    /// </summary>
    public static class GenericHelper
    {
        public static string GetEnumDescription<T>(this T value) where T : Enum
        {
            var field = value.GetType().GetField(value.ToString())!;
            var attributes = (DescriptionAttribute[])field.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes.Length > 0)
            {
                return attributes[0].Description;
            }
            else
            {
                return value.ToString();
            }
        }
    }
}
