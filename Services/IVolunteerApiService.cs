using VoluntariosConectadosRD.Models.DTOs;

namespace VoluntariosConectadosRD.Services
{
    public interface IVolunteerApiService
    {
        Task<ApiResponseDto<IEnumerable<OpportunityListDto>>?> GetVolunteerOpportunitiesAsync();
        Task<ApiResponseDto<IEnumerable<OpportunityListDto>>?> GetOrganizationOpportunitiesAsync();
        Task<ApiResponseDto<OpportunityDetailDto>?> GetOpportunityDetailsAsync(int id);
        Task<ApiResponseDto<OpportunityDetailDto>?> GetOpportunityByIdAsync(int id);
        Task<ApiResponseDto<object>?> ApplyToOpportunityAsync(int opportunityId, string? message = null);
        Task<ApiResponseDto<object>?> CreateOpportunityAsync(CreateOpportunityDto opportunity);
        Task<ApiResponseDto<object>?> UpdateOpportunityAsync(int opportunityId, UpdateOpportunityDto opportunity);
        Task<ApiResponseDto<object>?> GetApplicationsAsync();
        Task<ApiResponseDto<IEnumerable<VolunteerApplicationDetailDto>>?> GetMyApplicationsAsync();
        Task<ApiResponseDto<IEnumerable<VolunteerApplicationDetailDto>>?> GetVolunteerApplicationsAsync(int volunteerId);
        Task<ApiResponseDto<IEnumerable<VolunteerApplicationDetailDto>>?> GetApplicationsForOpportunityAsync(int opportunityId);
        Task<ApiResponseDto<object>?> UpdateApplicationStatusAsync(int applicationId, int status);
        Task<ApiResponseDto<AdminStatsDto>?> GetAdminStatsAsync();
        Task<ApiResponseDto<PaginatedResult<AdminVolunteerDto>>?> GetVolunteersForAdminAsync(int page = 1, int pageSize = 10, string? search = null);
    }
}