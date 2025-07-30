using System.ComponentModel.DataAnnotations;

namespace VoluntariosConectadosRD.Models
{
    public class RegistroViewModel
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(15, ErrorMessage = "El nombre no puede pasar de 15 caracteres")]
        public string? Nombre { get; set; }

        [Required(ErrorMessage = "El apellido es obligatorio")]
        [StringLength(15, ErrorMessage = "El apellido no puede pasar de 15 caracteres")]
        public string? Apellidos { get; set; }

        [Required(ErrorMessage = "El correo es obligatorio")]
        [EmailAddress(ErrorMessage = "Ingresa un correo válido")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "El teléfono es obligatorio")]
        [RegularExpression(@"^(\+?1\s?)?(\(?809\)?|\(?829\)?|\(?849\)?)[\s\-]?\d{3}[\s\-]?\d{4}$",
    ErrorMessage = "Use un formato de teléfono válido dominicano")]
        public string? Telefono { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [MinLength(8, ErrorMessage = "La contraseña debe tener al menos 8 caracteres")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "Confirma tu contraseña")]
        [Compare("Password", ErrorMessage = "Las contraseñas no coinciden")]
        public string? Confirmar { get; set; }

        [Required(ErrorMessage = "La fecha de nacimiento es requerida")]
        [DataType(DataType.Date)]
        public DateTime? Fecha { get; set; }

        [Required(ErrorMessage = "Su provincia es obligatoria")]
        public string? Provincia { get; set; }

        [Required(ErrorMessage = "Debes aceptar los Términos y Condiciones")]
        [Display(Name = "Terms and Conditions")]
        public bool Terminos { get; set; }
    }
} 
