using System.ComponentModel.DataAnnotations;

namespace VoluntariosConectadosRD.Models.DTOs
{
    // Application with Details DTO
    public class VolunteerApplicationDetailDto
    {
        public int Id { get; set; }
        public string OpportunityTitle { get; set; } = string.Empty;
        public string OrganizacionNombre { get; set; } = string.Empty;
        public DateTime FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public string? Ubicacion { get; set; }
        public ApplicationStatus Estado { get; set; }
        public DateTime FechaAplicacion { get; set; }
        public DateTime? FechaRespuesta { get; set; }
        public string? Mensaje { get; set; }
        public string? NotasOrganizacion { get; set; }
        public int HorasEstimadas { get; set; }
        public int? HorasCompletadas { get; set; }
    }

    // Admin Volunteer DTO
    public class AdminVolunteerDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Telefono { get; set; }
        public string? ImagenUrl { get; set; }
        public UserStatus Estado { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string? OrganizacionActual { get; set; }
        public int HorasVoluntariado { get; set; }
        public int EventosParticipados { get; set; }
        public decimal CalificacionPromedio { get; set; }
        public DateTime? UltimaActividad { get; set; }
        public string Pais { get; set; } = "República Dominicana";
    }

    // Admin Statistics DTO
    public class AdminStatsDto
    {
        public int TotalVoluntarios { get; set; }
        public int VoluntariosActivos { get; set; }
        public int VoluntariosInactivos { get; set; }
        public int VoluntariosSuspendidos { get; set; }
        public int TotalOrganizaciones { get; set; }
        public int OrganizacionesActivas { get; set; }
        public int TotalOportunidades { get; set; }
        public int OportunidadesActivas { get; set; }
        public int TotalAplicaciones { get; set; }
        public int AplicacionesPendientes { get; set; }
        public int AplicacionesAprobadas { get; set; }
        public int TotalHorasVoluntariado { get; set; }
        public DateTime FechaActualizacion { get; set; }
        public Dictionary<string, int> VoluntariosPorMes { get; set; } = new Dictionary<string, int>();
        public Dictionary<string, int> AplicacionesPorMes { get; set; } = new Dictionary<string, int>();
    }

    // Paginated Result DTO
    public class PaginatedResult<T>
    {
        public List<T> Items { get; set; } = new List<T>();
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
        public bool HasNextPage => Page < TotalPages;
        public bool HasPreviousPage => Page > 1;
    }

    // Organization Dashboard DTOs
    public class OrganizationStatsDto
    {
        public int ActiveDonors { get; set; }
        public int EventsRealized { get; set; }
        public decimal TotalDonations { get; set; }
        public int ActiveProjects { get; set; }
        public decimal AverageDonation { get; set; }
        public int NewDonors { get; set; }
        public decimal RecurrentDonorsPercentage { get; set; }
        public int PeopleBenefited { get; set; }
        public int CommunitiesReached { get; set; }
        public int GrowthPercentage { get; set; }
        public int NewCommunities { get; set; }
        public List<MonthlyDonationDto> MonthlyDonations { get; set; } = new List<MonthlyDonationDto>();
        public List<ImpactDistributionDto> ImpactDistribution { get; set; } = new List<ImpactDistributionDto>();
    }

    public class MonthlyDonationDto
    {
        public string Month { get; set; } = string.Empty;
        public decimal Amount { get; set; }
    }

    public class ImpactDistributionDto
    {
        public string Category { get; set; } = string.Empty;
        public decimal Percentage { get; set; }
        public string Color { get; set; } = string.Empty;
    }

    public class OrganizationEventDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public string Location { get; set; } = string.Empty;
        public int Participants { get; set; }
        public string Status { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public string IconColor { get; set; } = string.Empty;
    }

    // Platform Statistics DTO
    public class PlatformStatsDto
    {
        public int VoluntariosActivos { get; set; }
        public int OrganizacionesActivas { get; set; }
        public int ProyectosActivos { get; set; }
        public int HorasTotalesDonadas { get; set; }
        public int PersonasBeneficiadas { get; set; }
        public decimal FondosRecaudados { get; set; }
        public DateTime FechaActualizacion { get; set; }
    }

    // Admin DTOs for User Management
    public class AdminEditUserDto
    {
        [Required]
        public string Nombre { get; set; } = string.Empty;
        
        [Required]
        public string Apellido { get; set; } = string.Empty;
        
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        
        public string? Telefono { get; set; }
        
        public DateTime? FechaNacimiento { get; set; }
        
        public string? Biografia { get; set; }
        
        public string? Disponibilidad { get; set; }
        
        public UserStatus Status { get; set; }
    }

    // Admin DTOs for Organization Management
    public class AdminOrganizationDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Telefono { get; set; }
        public string? Direccion { get; set; }
        public string? SitioWeb { get; set; }
        public string? NumeroRegistro { get; set; }
        public bool Verificada { get; set; }
        public UserStatus Estado { get; set; }
        public DateTime FechaRegistro { get; set; }
        public DateTime? FechaVerificacion { get; set; }
        public string? LogoUrl { get; set; }
        public string? TipoOrganizacion { get; set; }
        public int TotalOportunidades { get; set; }
        public int OportunidadesActivas { get; set; }
        public int TotalVoluntarios { get; set; }
        public DateTime? UltimaActividad { get; set; }
    }

    public class AdminEditOrganizationDto
    {
        [Required]
        public string Nombre { get; set; } = string.Empty;
        
        public string? Descripcion { get; set; }
        
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        
        public string? Telefono { get; set; }
        
        public string? Direccion { get; set; }
        
        public string? SitioWeb { get; set; }
        
        public string? NumeroRegistro { get; set; }
        
        public string? TipoOrganizacion { get; set; }
        
        public UserStatus Status { get; set; }
        
        public bool Verificada { get; set; }
    }

}