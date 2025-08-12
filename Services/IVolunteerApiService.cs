using VoluntariosConectadosRD.Models.DTOs;
using BackendDTOs = VoluntariadoConectadoRD.Models.DTOs;

namespace VoluntariosConectadosRD.Services
{
    public interface IVolunteerApiService
    {
        Task<ApiResponseDto<IEnumerable<BackendDTOs.OpportunityListDto>>?> GetVolunteerOpportunitiesAsync();
        Task<ApiResponseDto<IEnumerable<BackendDTOs.OpportunityListDto>>?> GetOrganizationOpportunitiesAsync();
        Task<ApiResponseDto<BackendDTOs.OpportunityDetailDto>?> GetOpportunityDetailsAsync(int id);
        Task<ApiResponseDto<BackendDTOs.OpportunityDetailDto>?> GetOpportunityByIdAsync(int id);
        Task<ApiResponseDto<object>?> ApplyToOpportunityAsync(int opportunityId, string? message = null);
        Task<ApiResponseDto<object>?> CreateOpportunityAsync(BackendDTOs.CreateOpportunityDto opportunity);
        Task<ApiResponseDto<object>?> UpdateOpportunityAsync(int opportunityId, BackendDTOs.UpdateOpportunityDto opportunity);
        Task<ApiResponseDto<object>?> GetApplicationsAsync();
        Task<ApiResponseDto<IEnumerable<BackendDTOs.VolunteerApplicationDetailDto>>?> GetMyApplicationsAsync();
        Task<ApiResponseDto<IEnumerable<BackendDTOs.VolunteerApplicationDetailDto>>?> GetVolunteerApplicationsAsync(int volunteerId);
        Task<ApiResponseDto<IEnumerable<BackendDTOs.VolunteerApplicationDetailDto>>?> GetApplicationsForOpportunityAsync(int opportunityId);
        Task<ApiResponseDto<object>?> UpdateApplicationStatusAsync(int applicationId, int status);
        Task<ApiResponseDto<BackendDTOs.AdminStatsDto>?> GetAdminStatsAsync();
        Task<ApiResponseDto<PaginatedResult<BackendDTOs.AdminVolunteerDto>>?> GetVolunteersForAdminAsync(int page = 1, int pageSize = 10, string? search = null);
    }
}