using VoluntariosConectadosRD.Models;
using VoluntariosConectadosRD.Models.DTOs;
using BackendDTOs = VoluntariadoConectadoRD.Models.DTOs;

namespace VoluntariosConectadosRD.Services
{
    public interface IAccountApiService
    {
        Task<ApiResponseDto<BackendDTOs.LoginResponseDto>?> LoginAsync(LoginViewModel model);
        Task<ApiResponseDto<UserInfoDto>?> RegisterVolunteerAsync(RegistroViewModel model);
        Task<ApiResponseDto<UserInfoDto>?> RegisterONGAsync(RegistroONGViewModel model);
        Task<ApiResponseDto<BackendDTOs.UserProfileDto>?> UpdateVolunteerAsync(EditarVoluntarioViewModel model);
        Task<ApiResponseDto<BackendDTOs.OrganizationProfileDto>?> UpdateONGAsync(EditarONGViewModel model);
        Task<ApiResponseDto<UserInfoDto>?> GetUserProfileAsync();
        Task<ApiResponseDto<BackendDTOs.EnhancedUserProfileDto>?> GetUserProfileByIdAsync(int userId);
        Task<ApiResponseDto<BackendDTOs.OrganizationProfileDto>?> GetOrganizationProfileAsync(int orgId);
        Task<ApiResponseDto<bool>?> ValidateEmailAsync(string email);
        Task<ApiResponseDto<bool>?> ChangePasswordAsync(string currentPassword, string newPassword);
    }
} 