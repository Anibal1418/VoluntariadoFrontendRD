namespace VoluntariosConectadosRD.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Oportunidad
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El título es obligatorio")]
        [StringLength(100)]
        public string Titulo { get; set; }

        [Required(ErrorMessage = "La descripción es obligatoria")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "La fecha es obligatoria")]
        public DateTime Fecha { get; set; }

        [Required(ErrorMessage = "La ubicación es obligatoria")]
        public string Ubicacion { get; set; }
    }
}
