using VoluntariosConectadosRD.Models;
using VoluntariosConectadosRD.Models.DTOs;
using VoluntariadoConectadoRD.Models.DTOs;

namespace VoluntariosConectadosRD.Services
{
    public interface IAccountApiService
    {
        Task<ApiResponseDto<LoginResponseDto>?> LoginAsync(LoginViewModel model);
        Task<ApiResponseDto<UserInfoDto>?> RegisterVolunteerAsync(RegistroViewModel model);
        Task<ApiResponseDto<UserInfoDto>?> RegisterONGAsync(RegistroONGViewModel model);
        Task<ApiResponseDto<UserProfileDto>?> UpdateVolunteerAsync(EditarVoluntarioViewModel model);
        Task<ApiResponseDto<OrganizationProfileDto>?> UpdateONGAsync(EditarONGViewModel model);
        Task<ApiResponseDto<UserInfoDto>?> GetUserProfileAsync();
        Task<ApiResponseDto<EnhancedUserProfileDto>?> GetUserProfileByIdAsync(int userId);
        Task<ApiResponseDto<OrganizationProfileDto>?> GetOrganizationProfileAsync(int orgId);
        Task<ApiResponseDto<bool>?> ValidateEmailAsync(string email);
        Task<ApiResponseDto<bool>?> ChangePasswordAsync(string currentPassword, string newPassword);
    }
} 