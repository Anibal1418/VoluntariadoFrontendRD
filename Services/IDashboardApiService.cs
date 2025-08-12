using VoluntariosConectadosRD.Models.DTOs;
using VoluntariadoConectadoRD.Models.DTOs;

namespace VoluntariosConectadosRD.Services
{
    public interface IDashboardApiService
    {
        Task<ApiResponseDto<VoluntariosConectadosRD.Models.DTOs.DashboardStatsDto>?> GetDashboardStatsAsync();
        Task<ApiResponseDto<VoluntariosConectadosRD.Models.DTOs.UserDashboardDto>?> GetUserDashboardAsync();
        Task<ApiResponseDto<VoluntariadoConectadoRD.Models.DTOs.OrganizationDashboardDto>?> GetOrganizationDashboardAsync();
        Task<ApiResponseDto<IEnumerable<VoluntariosConectadosRD.Models.DTOs.RecentActivityDto>>?> GetRecentActivitiesAsync(int limit = 10);
        Task<ApiResponseDto<IEnumerable<VoluntariosConectadosRD.Models.DTOs.OpportunityListDto>>?> GetMyOpportunitiesAsync(int limit = 5);
        Task<ApiResponseDto<PaginatedResult<VoluntariosConectadosRD.Models.DTOs.AdminVolunteerDto>>?> GetVolunteersForReportsAsync(int page = 1, int pageSize = 10, string? search = null);
        Task<ApiResponseDto<VoluntariosConectadosRD.Models.DTOs.AdminStatsDto>?> GetAdminStatsForReportsAsync();
        Task<ApiResponseDto<VoluntariosConectadosRD.Models.DTOs.OrganizationStatsDto>?> GetOrganizationStatsAsync();
        Task<ApiResponseDto<IEnumerable<VoluntariosConectadosRD.Models.DTOs.OrganizationEventDto>>?> GetOrganizationEventsAsync();
        Task<ApiResponseDto<IEnumerable<VoluntariosConectadosRD.Models.DTOs.UserEventDto>>?> GetUserEventsAsync();
    }
}