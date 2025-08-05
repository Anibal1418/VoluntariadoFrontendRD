using System.ComponentModel.DataAnnotations;

namespace VoluntariosConectadosRD.Models
{
    public class FutureDateAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value is DateTime date)
            {
                // Validar que la fecha no sea inválida (como 31/09)
                try
                {
                    // Si llegamos aquí, la fecha es técnicamente válida
                    // Ahora verificamos que sea futura (al menos hoy)
                    return date.Date >= DateTime.Today;
                }
                catch
                {
                    return false;
                }
            }
            
            return false;
        }

        public override string FormatErrorMessage(string name)
        {
            return $"La fecha del {name} debe ser válida y no puede ser anterior a hoy.";
        }
    }
}