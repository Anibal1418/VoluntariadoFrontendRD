using System.ComponentModel.DataAnnotations;
//Chequeo de entradas de Login usando data annotations
namespace VoluntariosConectadosRD.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "El correo es obligatorio")]
        [EmailAddress(ErrorMessage = "Ingresa un correo válido")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un rol")]
        [Display(Name = "Tipo de Usuario")]
        public string? UserRole { get; set; }

        public bool Remember { get; set; }
    }
} 
