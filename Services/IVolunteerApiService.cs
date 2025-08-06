using VoluntariosConectadosRD.Models.DTOs;

namespace VoluntariosConectadosRD.Services
{
    public interface IVolunteerApiService
    {
        Task<ApiResponseDto<IEnumerable<OpportunityListDto>>?> GetVolunteerOpportunitiesAsync();
        Task<ApiResponseDto<OpportunityDetailDto>?> GetOpportunityDetailsAsync(int id);
        Task<ApiResponseDto<object>?> ApplyToOpportunityAsync(int opportunityId, string? message = null);
        Task<ApiResponseDto<object>?> CreateOpportunityAsync(CreateOpportunityDto opportunity);
        Task<ApiResponseDto<object>?> UpdateOpportunityAsync(int opportunityId, UpdateOpportunityDto opportunity);
        Task<ApiResponseDto<object>?> GetApplicationsAsync();
        Task<ApiResponseDto<IEnumerable<VolunteerApplicationDetailDto>>?> GetMyApplicationsAsync();
        Task<ApiResponseDto<AdminStatsDto>?> GetAdminStatsAsync();
        Task<ApiResponseDto<PaginatedResult<AdminVolunteerDto>>?> GetVolunteersForAdminAsync(int page = 1, int pageSize = 10, string? search = null);
    }
}