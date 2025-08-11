using VoluntariosConectadosRD.Models.DTOs;
using VoluntariadoConectadoRD.Models.DTOs;

namespace VoluntariosConectadosRD.Services
{
    public interface IAdminApiService
    {
        // User Management
        Task<ApiResponseDto<PaginatedResult<AdminVolunteerDto>>?> GetAllVolunteersAsync(int page = 1, int pageSize = 10, string? search = null);
        Task<ApiResponseDto<object>?> UpdateUserStatusAsync(int userId, UserStatus status);
        Task<ApiResponseDto<object>?> EditUserProfileAsync(int userId, AdminEditUserDto editDto);
        Task<ApiResponseDto<object>?> DeleteUserAsync(int userId);
        
        // Organization Management
        Task<ApiResponseDto<PaginatedResult<AdminOrganizationDto>>?> GetAllOrganizationsAsync(int page = 1, int pageSize = 10, string? search = null);
        Task<ApiResponseDto<object>?> UpdateOrganizationStatusAsync(int orgId, UserStatus status, bool verificada);
        Task<ApiResponseDto<object>?> EditOrganizationAsync(int orgId, AdminEditOrganizationDto editDto);
        Task<ApiResponseDto<object>?> DeleteOrganizationAsync(int orgId);
        
        // Statistics
        Task<ApiResponseDto<AdminStatsDto>?> GetAdminStatsAsync();
        Task<ApiResponseDto<PlatformStatsDto>?> GetPlatformStatsAsync();
    }
}