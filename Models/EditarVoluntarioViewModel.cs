using System.ComponentModel.DataAnnotations;

namespace VoluntariosConectadosRD.Models
{
    public class EditarVoluntarioViewModel
    {
        public int Id { get; set; }

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

        [Required(ErrorMessage = "La fecha de nacimiento es requerida")]
        [DataType(DataType.Date)]
        public DateTime? FechaNacimiento { get; set; }

        [Required(ErrorMessage = "Su provincia es obligatoria")]
        public string? Provincia { get; set; }

        [StringLength(200, ErrorMessage = "La descripción no puede pasar de 200 caracteres")]
        public string? Descripcion { get; set; }

        [Required(ErrorMessage = "La disponibilidad es obligatoria")]
        public string? Disponibilidad { get; set; }

        public string? FotoActual { get; set; }

        public string? NuevaFoto { get; set; }
    }
} 