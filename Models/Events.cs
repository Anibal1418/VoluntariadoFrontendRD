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

        [Required(ErrorMessage = "La fecha es obligatoria")]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha del Evento")]
        public DateTime Fecha { get; set; }

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
