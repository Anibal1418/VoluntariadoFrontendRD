using VoluntariosConectadosRD.Models;
using VoluntariosConectadosRD.Models.DTOs;
using VoluntariadoConectadoRD.Models.DTOs;

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

        public async Task<ApiResponseDto<LoginResponseDto>?> LoginAsync(LoginViewModel model)
        {
            var loginRequest = new LoginRequestDto
            {
                Email = model.Email,
                Password = model.Password
            };
            return await _baseApiService.PostAsync<ApiResponseDto<LoginResponseDto>>("auth/login", loginRequest);
        }

        public async Task<ApiResponseDto<UserInfoDto>?> RegisterVolunteerAsync(RegistroViewModel model)
        {
            var registerRequest = new RegisterVoluntarioDto
            {
                Nombre = model.Nombre ?? "",
                Apellido = model.Apellidos ?? "",
                Email = model.Email ?? "",
                Password = model.Password ?? "",
                Telefono = model.Telefono,
                Direccion = model.Provincia, // Usando Provincia como dirección
                FechaNacimiento = model.Fecha ?? DateTime.Now.AddYears(-18)
            };
            return await _baseApiService.PostAsync<ApiResponseDto<UserInfoDto>>("auth/register/voluntario", registerRequest);
        }

        public async Task<ApiResponseDto<UserInfoDto>?> RegisterONGAsync(RegistroONGViewModel model)
        {
            var registerRequest = new RegisterOrganizacionDto
            {
                NombreOrganizacion = model.NombreONG ?? "",
                DescripcionOrganizacion = model.Descripcion,
                EmailOrganizacion = model.Email ?? "",
                TelefonoOrganizacion = model.Telefono,
                DireccionOrganizacion = GetFullAddress(model.Direccion, model.Ciudad, model.Provincia),
                SitioWeb = null, // Not available in frontend model - could be added later
                NumeroRegistro = model.RNC,
                NombreAdmin = ExtractFirstName(model.NombreONG),
                ApellidoAdmin = "Admin",
                EmailAdmin = model.Email ?? "",
                PasswordAdmin = model.Password ?? "",
                TelefonoAdmin = model.Telefono,
                FechaNacimientoAdmin = DateTime.Now.AddYears(-30) // Default admin age
            };
            return await _baseApiService.PostAsync<ApiResponseDto<UserInfoDto>>("auth/register/organizacion", registerRequest);
        }

        public async Task<ApiResponseDto<UserProfileDto>?> UpdateVolunteerAsync(EditarVoluntarioViewModel model)
        {
            var updateRequest = new UpdateUserProfileDto
            {
                Nombre = model.Nombre,
                Apellido = model.Apellidos,
                Telefono = model.Telefono,
                FechaNacimiento = model.FechaNacimiento,
                Biografia = model.Descripcion,
                Disponibilidad = model.Disponibilidad
            };
            return await _baseApiService.PutAsync<ApiResponseDto<UserProfileDto>>("profile/user", updateRequest);
        }

        public async Task<ApiResponseDto<OrganizationProfileDto>?> UpdateONGAsync(EditarONGViewModel model)
        {
            var updateRequest = new UpdateOrganizationProfileDto
            {
                Nombre = model.NombreONG,
                Descripcion = model.Descripcion,
                Direccion = GetFullAddress(model.Direccion, model.Ciudad, model.Provincia),
                Telefono = model.Telefono,
                SitioWeb = null // Not available in frontend model - could be added later
                // Note: LogoONG field could be used here if the backend supports logo updates
            };
            return await _baseApiService.PutAsync<ApiResponseDto<OrganizationProfileDto>>("profile/organization", updateRequest);
        }

        public async Task<ApiResponseDto<UserInfoDto>?> GetUserProfileAsync()
        {
            return await _baseApiService.GetAsync<ApiResponseDto<UserInfoDto>>("auth/profile");
        }

        public async Task<ApiResponseDto<EnhancedUserProfileDto>?> GetUserProfileByIdAsync(int userId)
        {
            return await _baseApiService.GetAsync<ApiResponseDto<EnhancedUserProfileDto>>($"volunteer/profile/{userId}");
        }

        public async Task<ApiResponseDto<OrganizationProfileDto>?> GetOrganizationProfileAsync(int orgId)
        {
            return await _baseApiService.GetAsync<ApiResponseDto<OrganizationProfileDto>>($"profile/organization/{orgId}");
        }

        public async Task<ApiResponseDto<bool>?> ValidateEmailAsync(string email)
        {
            return await _baseApiService.GetAsync<ApiResponseDto<bool>>($"auth/validate-email?email={Uri.EscapeDataString(email)}");
        }

        public async Task<ApiResponseDto<bool>?> ChangePasswordAsync(string currentPassword, string newPassword)
        {
            var changePasswordRequest = new
            {
                CurrentPassword = currentPassword,
                NewPassword = newPassword
            };
            return await _baseApiService.PostAsync<ApiResponseDto<bool>>("auth/change-password", changePasswordRequest);
        }

        private static string GetFullAddress(string? direccion, string? ciudad, string? provincia)
        {
            var addressParts = new List<string>();
            
            if (!string.IsNullOrEmpty(direccion))
                addressParts.Add(direccion);
            if (!string.IsNullOrEmpty(ciudad))
                addressParts.Add(ciudad);
            if (!string.IsNullOrEmpty(provincia))
                addressParts.Add(provincia);
            
            return string.Join(", ", addressParts);
        }

        private static string ExtractFirstName(string? organizationName)
        {
            if (string.IsNullOrEmpty(organizationName))
                return "Admin";
            
            // Extract first word as first name for admin user
            var parts = organizationName.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            return parts.Length > 0 ? parts[0] : "Admin";
        }
    }
} 