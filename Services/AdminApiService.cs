using VoluntariosConectadosRD.Models.DTOs;

namespace VoluntariosConectadosRD.Services
{
    public class AdminApiService : IAdminApiService
    {
        private readonly IBaseApiService _baseApiService;
        private readonly ILogger<AdminApiService> _logger;

        public AdminApiService(IBaseApiService baseApiService, ILogger<AdminApiService> logger)
        {
            _baseApiService = baseApiService;
            _logger = logger;
        }

        // User Management
        public async Task<ApiResponseDto<PaginatedResult<AdminVolunteerDto>>?> GetAllVolunteersAsync(int page = 1, int pageSize = 10, string? search = null)
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
                _logger.LogError(ex, "Error calling get all volunteers API");
                return null;
            }
        }

        public async Task<ApiResponseDto<object>?> UpdateUserStatusAsync(int userId, UserStatus status)
        {
            try
            {
                var statusDto = new { Status = status };
                return await _baseApiService.PutAsync<ApiResponseDto<object>>($"Volunteer/admin/users/{userId}/status", statusDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calling update user status API for user {UserId}", userId);
                return null;
            }
        }

        public async Task<ApiResponseDto<object>?> EditUserProfileAsync(int userId, AdminEditUserDto editDto)
        {
            try
            {
                return await _baseApiService.PutAsync<ApiResponseDto<object>>($"Volunteer/admin/users/{userId}", editDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calling edit user profile API for user {UserId}", userId);
                return null;
            }
        }

        public async Task<ApiResponseDto<object>?> DeleteUserAsync(int userId)
        {
            try
            {
                var success = await _baseApiService.DeleteAsync($"Volunteer/admin/users/{userId}");
                return new ApiResponseDto<object>
                {
                    Success = success,
                    Message = success ? "Usuario eliminado exitosamente" : "Error al eliminar usuario",
                    Data = null
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calling delete user API for user {UserId}", userId);
                return null;
            }
        }

        // Organization Management
        public async Task<ApiResponseDto<PaginatedResult<AdminOrganizationDto>>?> GetAllOrganizationsAsync(int page = 1, int pageSize = 10, string? search = null)
        {
            try
            {
                var queryParams = $"?page={page}&pageSize={pageSize}";
                if (!string.IsNullOrEmpty(search))
                {
                    queryParams += $"&search={Uri.EscapeDataString(search)}";
                }
                
                return await _baseApiService.GetAsync<ApiResponseDto<PaginatedResult<AdminOrganizationDto>>>($"Volunteer/admin/organizations{queryParams}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calling get all organizations API");
                return null;
            }
        }

        public async Task<ApiResponseDto<object>?> UpdateOrganizationStatusAsync(int orgId, UserStatus status, bool verificada)
        {
            try
            {
                var statusDto = new { Status = status, Verificada = verificada };
                return await _baseApiService.PutAsync<ApiResponseDto<object>>($"Volunteer/admin/organizations/{orgId}/status", statusDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calling update organization status API for org {OrgId}", orgId);
                return null;
            }
        }

        public async Task<ApiResponseDto<object>?> EditOrganizationAsync(int orgId, AdminEditOrganizationDto editDto)
        {
            try
            {
                return await _baseApiService.PutAsync<ApiResponseDto<object>>($"Volunteer/admin/organizations/{orgId}", editDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calling edit organization API for org {OrgId}", orgId);
                return null;
            }
        }

        public async Task<ApiResponseDto<object>?> DeleteOrganizationAsync(int orgId)
        {
            try
            {
                var success = await _baseApiService.DeleteAsync($"Volunteer/admin/organizations/{orgId}");
                return new ApiResponseDto<object>
                {
                    Success = success,
                    Message = success ? "Organización eliminada exitosamente" : "Error al eliminar organización",
                    Data = null
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calling delete organization API for org {OrgId}", orgId);
                return null;
            }
        }

        // Statistics
        public async Task<ApiResponseDto<AdminStatsDto>?> GetAdminStatsAsync()
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

        public async Task<ApiResponseDto<PlatformStatsDto>?> GetPlatformStatsAsync()
        {
            try
            {
                return await _baseApiService.GetAsync<ApiResponseDto<PlatformStatsDto>>("Volunteer/platform/stats");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calling platform stats API");
                return null;
            }
        }
    }
}