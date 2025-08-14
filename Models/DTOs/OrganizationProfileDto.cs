namespace VoluntariosConectadosRD.Models.DTOs
{
    public class OrganizationProfileDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public string? Direccion { get; set; }
        public string? Telefono { get; set; }
        public string Email { get; set; } = string.Empty;
        public string? SitioWeb { get; set; }
        public string? TipoOrganizacion { get; set; }
        public string? NumeroRegistro { get; set; }
        public DateTime? FechaFundacion { get; set; }
        public string? Mision { get; set; }
        public string? Vision { get; set; }
        public List<string> AreasInteres { get; set; } = new List<string>();
        public string? LogoUrl { get; set; }
        public bool PerfilCompleto { get; set; }
        public decimal SaldoActual { get; set; } = 0m;
    }
}