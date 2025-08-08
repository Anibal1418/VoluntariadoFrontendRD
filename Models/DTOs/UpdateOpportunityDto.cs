using System.ComponentModel.DataAnnotations;

namespace VoluntariosConectadosRD.Models.DTOs
{
    public class UpdateOpportunityDto
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "El título es requerido")]
        [StringLength(200, ErrorMessage = "El título no puede exceder 200 caracteres")]
        public string Titulo { get; set; } = string.Empty;

        [Required(ErrorMessage = "La descripción es requerida")]
        [StringLength(1000, ErrorMessage = "La descripción no puede exceder 1000 caracteres")]
        public string Descripcion { get; set; } = string.Empty;

        [Required(ErrorMessage = "La fecha de inicio es requerida")]
        public DateTime FechaInicio { get; set; }

        public DateTime? FechaFin { get; set; }

        [Required(ErrorMessage = "La ubicación es requerida")]
        [StringLength(300, ErrorMessage = "La ubicación no puede exceder 300 caracteres")]
        public string Ubicacion { get; set; } = string.Empty;

        [Range(1, 48, ErrorMessage = "La duración debe estar entre 1 y 48 horas")]
        public int DuracionHoras { get; set; }

        [Range(1, 1000, ErrorMessage = "El número de voluntarios debe estar entre 1 y 1000")]
        public int VoluntariosRequeridos { get; set; }
    }
}