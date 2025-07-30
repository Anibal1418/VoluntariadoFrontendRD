using System.ComponentModel.DataAnnotations;

namespace VoluntariosConectadosRD.Models
{
    public class RegistroONGViewModel
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(30, ErrorMessage = "El nombre no puede pasar de 30 caracteres")]
        public string? NombreONG { get; set; }

        [Required(ErrorMessage = "El RNC es obligatorio")]
        [StringLength(9, MinimumLength = 9, ErrorMessage = "El RNC debe tener 9 dígitos")]
        [RegularExpression(@"^\d{9}$", ErrorMessage = "El RNC debe contener solo 9 dígitos numéricos")]
        public string? RNC { get; set; }

        [Required(ErrorMessage = "El teléfono es obligatorio")]
        [RegularExpression(@"^(\+?1\s?)?(\(?809\)?|\(?829\)?|\(?849\)?)[\s\-]?\d{3}[\s\-]?\d{4}$",
    ErrorMessage = "Use un formato de teléfono válido dominicano")]
        public string? Telefono { get; set; }

        [Required(ErrorMessage = "El correo es obligatorio")]
        [EmailAddress(ErrorMessage = "Ingresa un correo válido")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [MinLength(8, ErrorMessage = "La contraseña debe tener al menos 8 caracteres")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "Confirma tu contraseña")]
        [Compare("Password", ErrorMessage = "Las contraseñas no coinciden")]
        public string? Confirmar { get; set; }

        public string? Direccion { get; set; }
        public string? Ciudad { get; set; }
        public string? Provincia { get; set; }

        [Required(ErrorMessage = "La descripción es obligatoria")]
        [StringLength(50, ErrorMessage = "la descripcion no puede pasar de 50 caracteres")]
        public string? Descripcion { get; set; }

        [Required(ErrorMessage = "El sector es obligatorio")]
        public string? Sector { get; set; }

        public string? LogoONG { get; set; }
        
        [Required(ErrorMessage = "Debes aceptar los Términos y Condiciones")]
        [Display(Name = "Terms and Conditions")]
        public bool Terminos { get; set; }
    }
} 
