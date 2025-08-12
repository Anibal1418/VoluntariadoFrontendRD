using System.ComponentModel.DataAnnotations;

namespace VoluntariadoConectadoRD.Models.DTOs
{
    public class UpdateOpportunityDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Titulo { get; set; } = string.Empty;

        [Required]
        [StringLength(2000)]
        public string Descripcion { get; set; } = string.Empty;

        [Required]
        public DateTime FechaInicio { get; set; }

        public DateTime? FechaFin { get; set; }

        [Required]
        [StringLength(200)]
        public string Ubicacion { get; set; } = string.Empty;

        [Range(1, 100)]
        public int DuracionHoras { get; set; }

        [Range(1, 1000)]
        public int VoluntariosRequeridos { get; set; }
    }
}