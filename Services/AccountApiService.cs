using VoluntariosConectadosRD.Models;

namespace VoluntariosConectadosRD.Services
{
    public class AccountApiService : IAccountApiService
    {
        private readonly IBaseApiService _baseApiService;
        private readonly ILogger<AccountApiService> _logger;

        public AccountApiService(IBaseApiService baseApiService, ILogger<AccountApiService> logger)
        {
            _baseApiService = baseApiService;
            _logger = logger;
        }

        public async Task<ApiResponse<LoginResponse>?> LoginAsync(LoginViewModel model)
        {
            return await _baseApiService.PostAsync<ApiResponse<LoginResponse>>("auth/login", model);
        }

        public async Task<ApiResponse<RegisterResponse>?> RegisterVolunteerAsync(RegistroViewModel model)
        {
            return await _baseApiService.PostAsync<ApiResponse<RegisterResponse>>("auth/register/volunteer", model);
        }

        public async Task<ApiResponse<RegisterResponse>?> RegisterONGAsync(RegistroONGViewModel model)
        {
            return await _baseApiService.PostAsync<ApiResponse<RegisterResponse>>("auth/register/ong", model);
        }
    }
} 