using VoluntariosConectadosRD.Models.DTOs;

namespace VoluntariosConectadosRD.Services
{
    public class VolunteerApiService : IVolunteerApiService
    {
        private readonly IBaseApiService _baseApiService;
        private readonly ILogger<VolunteerApiService> _logger;

        public VolunteerApiService(IBaseApiService baseApiService, ILogger<VolunteerApiService> logger)
        {
            _baseApiService = baseApiService;
            _logger = logger;
        }

        public async Task<ApiResponseDto<IEnumerable<OpportunityListDto>>?> GetVolunteerOpportunitiesAsync()
        {
            return await _baseApiService.GetAsync<ApiResponseDto<IEnumerable<OpportunityListDto>>>("voluntariado/opportunities");
        }

        public async Task<ApiResponseDto<IEnumerable<OpportunityListDto>>?> GetOrganizationOpportunitiesAsync()
        {
            return await _baseApiService.GetAsync<ApiResponseDto<IEnumerable<OpportunityListDto>>>("api/Voluntariado/organization-opportunities");
        }

        public async Task<ApiResponseDto<OpportunityDetailDto>?> GetOpportunityDetailsAsync(int id)
        {
            return await _baseApiService.GetAsync<ApiResponseDto<OpportunityDetailDto>>($"voluntariado/opportunities/{id}");
        }

        public async Task<ApiResponseDto<OpportunityDetailDto>?> GetOpportunityByIdAsync(int id)
        {
            return await _baseApiService.GetAsync<ApiResponseDto<OpportunityDetailDto>>($"api/Voluntariado/opportunities/{id}");
        }

        public async Task<ApiResponseDto<object>?> ApplyToOpportunityAsync(int opportunityId, string? message = null)
        {
            var applyDto = new ApplyToOpportunityDto { Message = message };
            return await _baseApiService.PostAsync<ApiResponseDto<object>>($"voluntariado/apply/{opportunityId}", applyDto);
        }

        public async Task<ApiResponseDto<object>?> CreateOpportunityAsync(CreateOpportunityDto opportunity)
        {
            return await _baseApiService.PostAsync<ApiResponseDto<object>>("voluntariado/opportunities", opportunity);
        }

        public async Task<ApiResponseDto<object>?> UpdateOpportunityAsync(int opportunityId, UpdateOpportunityDto opportunity)
        {
            return await _baseApiService.PutAsync<ApiResponseDto<object>>($"voluntariado/opportunities/{opportunityId}", opportunity);
        }

        public async Task<ApiResponseDto<object>?> GetApplicationsAsync()
        {
            return await _baseApiService.GetAsync<ApiResponseDto<object>>("voluntariado/applications");
        }

        public async Task<ApiResponseDto<IEnumerable<VolunteerApplicationDetailDto>>?> GetMyApplicationsAsync()
        {
            return await _baseApiService.GetAsync<ApiResponseDto<IEnumerable<VolunteerApplicationDetailDto>>>("Volunteer/applications/me");
        }

        public async Task<ApiResponseDto<IEnumerable<VolunteerApplicationDetailDto>>?> GetVolunteerApplicationsAsync(int volunteerId)
        {
            return await _baseApiService.GetAsync<ApiResponseDto<IEnumerable<VolunteerApplicationDetailDto>>>($"Volunteer/applications/{volunteerId}");
        }

        public async Task<ApiResponseDto<AdminStatsDto>?> GetAdminStatsAsync()
        {
            return await _baseApiService.GetAsync<ApiResponseDto<AdminStatsDto>>("Volunteer/admin/stats");
        }

        public async Task<ApiResponseDto<PaginatedResult<AdminVolunteerDto>>?> GetVolunteersForAdminAsync(int page = 1, int pageSize = 10, string? search = null)
        {
            var queryParams = $"?page={page}&pageSize={pageSize}";
            if (!string.IsNullOrEmpty(search))
            {
                queryParams += $"&search={Uri.EscapeDataString(search)}";
            }
            
            return await _baseApiService.GetAsync<ApiResponseDto<PaginatedResult<AdminVolunteerDto>>>($"Volunteer/admin/volunteers{queryParams}");
        }

        public async Task<ApiResponseDto<IEnumerable<VolunteerApplicationDetailDto>>?> GetApplicationsForOpportunityAsync(int opportunityId)
        {
            return await _baseApiService.GetAsync<ApiResponseDto<IEnumerable<VolunteerApplicationDetailDto>>>($"api/Voluntariado/opportunities/{opportunityId}/applications");
        }

        public async Task<ApiResponseDto<object>?> UpdateApplicationStatusAsync(int applicationId, int status)
        {
            return await _baseApiService.PutAsync<ApiResponseDto<object>>($"api/Voluntariado/applications/{applicationId}/status", new { status });
        }
    }
}