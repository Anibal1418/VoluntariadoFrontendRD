using System.ComponentModel.DataAnnotations;

namespace VoluntariosConectadosRD.Models.DTOs
{
    public class OpportunityListDto
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public string? Ubicacion { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public int DuracionHoras { get; set; }
        public int VoluntariosRequeridos { get; set; }
        public int VoluntariosInscritos { get; set; }
        public string? AreaInteres { get; set; }
        public string? NivelExperiencia { get; set; }
        public OpportunityStatus Estatus { get; set; }
        public OrganizacionBasicDto Organizacion { get; set; } = null!;
        
        // Compatibility properties for existing views
        public string OrganizacionNombre => Organizacion?.Nombre ?? "Sin especificar";
        public int VoluntariosRegistrados => VoluntariosInscritos;
        public int VoluntariosNecesarios => VoluntariosRequeridos;
    }

    public class OpportunityDetailDto : OpportunityListDto
    {
        public string? Requisitos { get; set; }
        public string? Beneficios { get; set; }
        public DateTime FechaCreacion { get; set; }
    }

    public class CreateOpportunityDto
    {
        [Required]
        [StringLength(200)]
        public string Titulo { get; set; } = string.Empty;

        [Required]
        [StringLength(2000)]
        public string Descripcion { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Ubicacion { get; set; }

        [Required]
        public DateTime FechaInicio { get; set; }

        public DateTime? FechaFin { get; set; }

        [Required]
        [Range(1, 1000)]
        public int DuracionHoras { get; set; }

        [Required]
        [Range(1, 500)]
        public int VoluntariosRequeridos { get; set; }

        [StringLength(100)]
        public string? AreaInteres { get; set; }

        [StringLength(50)]
        public string? NivelExperiencia { get; set; }

        [StringLength(1000)]
        public string? Requisitos { get; set; }

        [StringLength(1000)]
        public string? Beneficios { get; set; }
    }

    public class UpdateOpportunityDto : CreateOpportunityDto
    {
        public int Id { get; set; }
        public OpportunityStatus? Estatus { get; set; }
    }

    public class ApplyToOpportunityDto
    {
        [StringLength(1000)]
        public string? Mensaje { get; set; }
        
        // Alias for compatibility with existing service code
        public string? Message 
        { 
            get => Mensaje; 
            set => Mensaje = value; 
        }
    }

    public class OrganizacionBasicDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Ubicacion { get; set; }
    }

    public class ApplicationDto
    {
        public int Id { get; set; }
        public int OpportunityId { get; set; }
        public string OpportunityTitulo { get; set; } = string.Empty;
        public string UsuarioNombre { get; set; } = string.Empty;
        public string UsuarioEmail { get; set; } = string.Empty;
        public string? Mensaje { get; set; }
        public ApplicationStatus Estatus { get; set; }
        public DateTime FechaAplicacion { get; set; }
        public DateTime? FechaRespuesta { get; set; }
        public string? NotasOrganizacion { get; set; }
    }

    public class UpdateApplicationStatusDto
    {
        [Required]
        public ApplicationStatus Status { get; set; }

        [StringLength(500)]
        public string? Notes { get; set; }
    }

    public class OpportunityFilterDto
    {
        public string? SearchTerm { get; set; }
        public string? AreaInteres { get; set; }
        public string? Ubicacion { get; set; }
        public OpportunityStatus? Status { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public enum OpportunityStatus
    {
        Activa = 1,
        Inactiva = 2,
        Cerrada = 3,
        Pendiente = 4
    }

    public enum ApplicationStatus
    {
        Pendiente = 0,
        Aceptada = 1,
        Rechazada = 2,
        Retirada = 3
    }

    // Dashboard DTOs
    public class DashboardStatsDto
    {
        public int TotalUsers { get; set; }
        public int TotalOrganizations { get; set; }
        public int TotalOpportunities { get; set; }
        public int ActiveOpportunities { get; set; }
        public int TotalApplications { get; set; }
        public int CompletedOpportunities { get; set; }
        public double AverageRating { get; set; }
        public int TotalVolunteerHours { get; set; }
    }

    public class UserDashboardDto
    {
        public int UserId { get; set; }
        public int ApplicationsCount { get; set; }
        public int ApprovedApplications { get; set; }
        public int CompletedActivities { get; set; }
        public int VolunteerHours { get; set; }
        public double AverageRating { get; set; }
        public int BadgesCount { get; set; }
        public List<RecentActivityDto> RecentActivities { get; set; } = new List<RecentActivityDto>();
        public List<OpportunityListDto> SuggestedOpportunities { get; set; } = new List<OpportunityListDto>();
    }

    public class OrganizationDashboardDto
    {
        public int OrganizationId { get; set; }
        public int OpportunitiesCreated { get; set; }
        public int ActiveOpportunities { get; set; }
        public int TotalApplications { get; set; }
        public int ApprovedApplications { get; set; }
        public int TotalVolunteers { get; set; }
        public double AverageRating { get; set; }
        public List<RecentApplicationDto> RecentApplications { get; set; } = new List<RecentApplicationDto>();
        public List<OpportunityListDto> RecentOpportunities { get; set; } = new List<OpportunityListDto>();
    }

    public class RecentActivityDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ActivityType { get; set; } = string.Empty; // "applied", "approved", "completed", "created"
        public DateTime Date { get; set; }
        public string Status { get; set; } = string.Empty;
        public string OrganizationName { get; set; } = string.Empty;
        public string OpportunityTitle { get; set; } = string.Empty;
    }

    public class RecentApplicationDto
    {
        public int Id { get; set; }
        public string VolunteerName { get; set; } = string.Empty;
        public string OpportunityTitle { get; set; } = string.Empty;
        public DateTime ApplicationDate { get; set; }
        public ApplicationStatus Status { get; set; }
        public string? Message { get; set; }
    }

    public class UserEventDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string OrganizationName { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public DateTime? EndDate { get; set; }
        public string Location { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Requirements { get; set; }
        public string? ImageUrl { get; set; }
        public ApplicationStatus Status { get; set; }
    }
}