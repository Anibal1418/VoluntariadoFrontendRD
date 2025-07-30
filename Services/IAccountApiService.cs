using VoluntariosConectadosRD.Models;

namespace VoluntariosConectadosRD.Services
{
    public interface IAccountApiService
    {
        Task<ApiResponse<LoginResponse>?> LoginAsync(LoginViewModel model);
        Task<ApiResponse<RegisterResponse>?> RegisterVolunteerAsync(RegistroViewModel model);
        Task<ApiResponse<RegisterResponse>?> RegisterONGAsync(RegistroONGViewModel model);
        Task<ApiResponse<RegisterResponse>?> UpdateVolunteerAsync(EditarVoluntarioViewModel model);
        Task<ApiResponse<RegisterResponse>?> UpdateONGAsync(EditarONGViewModel model);
    }
} 