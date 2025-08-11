using VoluntariosConectadosRD.Models.DTOs;
using VoluntariadoConectadoRD.Models.DTOs;

namespace VoluntariosConectadosRD.Services
{
    public interface IDashboardApiService
    {
        Task<ApiResponseDto<DashboardStatsDto>?> GetDashboardStatsAsync();
        Task<ApiResponseDto<UserDashboardDto>?> GetUserDashboardAsync();
        Task<ApiResponseDto<OrganizationDashboardDto>?> GetOrganizationDashboardAsync();
        Task<ApiResponseDto<IEnumerable<RecentActivityDto>>?> GetRecentActivitiesAsync(int limit = 10);
        Task<ApiResponseDto<IEnumerable<OpportunityListDto>>?> GetMyOpportunitiesAsync(int limit = 5);
        Task<ApiResponseDto<PaginatedResult<AdminVolunteerDto>>?> GetVolunteersForReportsAsync(int page = 1, int pageSize = 10, string? search = null);
        Task<ApiResponseDto<AdminStatsDto>?> GetAdminStatsForReportsAsync();
        Task<ApiResponseDto<OrganizationStatsDto>?> GetOrganizationStatsAsync();
        Task<ApiResponseDto<IEnumerable<OrganizationEventDto>>?> GetOrganizationEventsAsync();
        Task<ApiResponseDto<IEnumerable<UserEventDto>>?> GetUserEventsAsync();
    }
}