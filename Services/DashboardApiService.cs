using VoluntariosConectadosRD.Models.DTOs;

namespace VoluntariosConectadosRD.Services
{
    public class DashboardApiService : IDashboardApiService
    {
        private readonly IBaseApiService _baseApiService;
        private readonly ILogger<DashboardApiService> _logger;

        public DashboardApiService(IBaseApiService baseApiService, ILogger<DashboardApiService> logger)
        {
            _baseApiService = baseApiService;
            _logger = logger;
        }

        public async Task<ApiResponseDto<DashboardStatsDto>?> GetDashboardStatsAsync()
        {
            try
            {
                return await _baseApiService.GetAsync<ApiResponseDto<DashboardStatsDto>>("Dashboard/stats");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calling dashboard stats API");
                return null;
            }
        }

        public async Task<ApiResponseDto<UserDashboardDto>?> GetUserDashboardAsync()
        {
            try
            {
                return await _baseApiService.GetAsync<ApiResponseDto<UserDashboardDto>>("Dashboard/user");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calling user dashboard API");
                return null;
            }
        }

        public async Task<ApiResponseDto<OrganizationDashboardDto>?> GetOrganizationDashboardAsync()
        {
            try
            {
                return await _baseApiService.GetAsync<ApiResponseDto<OrganizationDashboardDto>>("Dashboard/organization");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calling organization dashboard API");
                return null;
            }
        }

        public async Task<ApiResponseDto<IEnumerable<RecentActivityDto>>?> GetRecentActivitiesAsync(int limit = 10)
        {
            try
            {
                return await _baseApiService.GetAsync<ApiResponseDto<IEnumerable<RecentActivityDto>>>($"Dashboard/activities?limit={limit}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calling recent activities API");
                return null;
            }
        }

        public async Task<ApiResponseDto<IEnumerable<OpportunityListDto>>?> GetMyOpportunitiesAsync(int limit = 5)
        {
            try
            {
                return await _baseApiService.GetAsync<ApiResponseDto<IEnumerable<OpportunityListDto>>>($"Dashboard/my-opportunities?limit={limit}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calling my opportunities API");
                return null;
            }
        }

        public async Task<ApiResponseDto<PaginatedResult<AdminVolunteerDto>>?> GetVolunteersForReportsAsync(int page = 1, int pageSize = 10, string? search = null)
        {
            try
            {
                var queryParams = $"?page={page}&pageSize={pageSize}";
                if (!string.IsNullOrEmpty(search))
                {
                    queryParams += $"&search={Uri.EscapeDataString(search)}";
                }
                
                return await _baseApiService.GetAsync<ApiResponseDto<PaginatedResult<AdminVolunteerDto>>>($"Volunteer/admin/volunteers{queryParams}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calling volunteers for reports API");
                return null;
            }
        }

        public async Task<ApiResponseDto<AdminStatsDto>?> GetAdminStatsForReportsAsync()
        {
            try
            {
                return await _baseApiService.GetAsync<ApiResponseDto<AdminStatsDto>>("Volunteer/admin/stats");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calling admin stats API");
                return null;
            }
        }

        public async Task<ApiResponseDto<OrganizationStatsDto>?> GetOrganizationStatsAsync()
        {
            try
            {
                return await _baseApiService.GetAsync<ApiResponseDto<OrganizationStatsDto>>("Voluntariado/organization/stats");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calling organization stats API");
                return null;
            }
        }

        public async Task<ApiResponseDto<IEnumerable<OrganizationEventDto>>?> GetOrganizationEventsAsync()
        {
            try
            {
                return await _baseApiService.GetAsync<ApiResponseDto<IEnumerable<OrganizationEventDto>>>("Voluntariado/organization/events");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calling organization events API");
                return null;
            }
        }

        public async Task<ApiResponseDto<IEnumerable<UserEventDto>>?> GetUserEventsAsync()
        {
            try
            {
                return await _baseApiService.GetAsync<ApiResponseDto<IEnumerable<UserEventDto>>>("Voluntariado/user/events");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calling user events API");
                return null;
            }
        }
    }
}