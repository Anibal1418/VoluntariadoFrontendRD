using System;
using System.ComponentModel.DataAnnotations;

namespace VoluntariosConectadosRD.Models
{
    public class Events
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [Display(Name = "Nombre del Evento")]
        public required string Nombre { get; set; }

        [Required(ErrorMessage = "La fecha de inicio es obligatoria")]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de Inicio")]
        [FutureDate(ErrorMessage = "La fecha del evento debe ser futura")]
        public DateTime Fecha { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Fecha de Fin (Opcional)")]
        public DateTime? FechaFin { get; set; }

        [Required(ErrorMessage = "La duración es obligatoria")]
        [Range(1, 24, ErrorMessage = "La duración debe estar entre 1 y 24 horas")]
        [Display(Name = "Duración (Horas)")]
        public int DuracionHoras { get; set; } = 4; // Default 4 hours

        [Required(ErrorMessage = "El número de voluntarios es obligatorio")]
        [Range(1, 100, ErrorMessage = "Debe requerir entre 1 y 100 voluntarios")]
        [Display(Name = "Voluntarios Requeridos")]
        public int VoluntariosRequeridos { get; set; } = 5; // Default 5 volunteers

        [Required(ErrorMessage = "La ubicación es obligatoria")]
        [Display(Name = "Ubicación")]
        public required string Ubicacion { get; set; }

        [Display(Name = "Descripción")]
        public string? Descripcion { get; set; }

        [Display(Name = "Organizador")]
        public string? Organizador { get; set; }

        [Display(Name = "Imagen del Evento")]
        public string? ImagenUrl { get; set; }

        [Display(Name = "Nombre de la Imagen")]
        public string? ImagenNombre { get; set; }
    }
}
