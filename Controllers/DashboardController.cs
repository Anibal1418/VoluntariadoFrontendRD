using Microsoft.AspNetCore.Mvc;
using VoluntariosConectadosRD.Services;
using VoluntariosConectadosRD.Models.DTOs;
using System.Text.Json;

namespace VoluntariosConectadosRD.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IAccountApiService _accountApiService;
        private readonly IVolunteerApiService _volunteerApiService;
        private readonly IDashboardApiService _dashboardApiService;
        private readonly ILogger<DashboardController> _logger;

        public DashboardController(
            IAccountApiService accountApiService, 
            IVolunteerApiService volunteerApiService, 
            IDashboardApiService dashboardApiService,
            ILogger<DashboardController> logger)
        {
            _accountApiService = accountApiService;
            _volunteerApiService = volunteerApiService;
            _dashboardApiService = dashboardApiService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                // Get user info from session
                var userInfoJson = HttpContext.Session.GetString("UserInfo");
                if (string.IsNullOrEmpty(userInfoJson))
                {
                    return RedirectToAction("Login", "Account");
                }

                var userInfo = JsonSerializer.Deserialize<UserInfoDto>(userInfoJson);
                if (userInfo == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                ViewBag.UserInfo = userInfo;

                // Get dashboard stats (public endpoint, no auth required)
                try
                {
                    var statsResponse = await _dashboardApiService.GetDashboardStatsAsync();
                    if (statsResponse?.Success == true && statsResponse.Data != null)
                    {
                        ViewBag.DashboardStats = statsResponse.Data;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error getting dashboard stats");
                }

                // Get user-specific or organization-specific dashboard data based on role
                if (userInfo.Rol == UserRole.Organizacion && userInfo.Organizacion != null)
                {
                    // Organization dashboard
                    try
                    {
                        var orgDashboardResponse = await _dashboardApiService.GetOrganizationDashboardAsync();
                        if (orgDashboardResponse?.Success == true && orgDashboardResponse.Data != null)
                        {
                            ViewBag.OrganizationDashboard = orgDashboardResponse.Data;
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error getting organization dashboard data");
                    }
                }
                else if (userInfo.Rol == UserRole.Voluntario)
                {
                    // User dashboard
                    try
                    {
                        var userDashboardResponse = await _dashboardApiService.GetUserDashboardAsync();
                        if (userDashboardResponse?.Success == true && userDashboardResponse.Data != null)
                        {
                            ViewBag.UserDashboard = userDashboardResponse.Data;
                        }

                        var activitiesResponse = await _dashboardApiService.GetRecentActivitiesAsync(5);
                        if (activitiesResponse?.Success == true && activitiesResponse.Data != null)
                        {
                            ViewBag.RecentActivities = activitiesResponse.Data;
                        }

                        var opportunitiesResponse = await _dashboardApiService.GetMyOpportunitiesAsync(3);
                        if (opportunitiesResponse?.Success == true && opportunitiesResponse.Data != null)
                        {
                            ViewBag.MyOpportunities = opportunitiesResponse.Data;
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error getting user dashboard data");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in dashboard index");
                ViewBag.ErrorMessage = "Error al cargar el dashboard.";
                
                // Fallback to basic session data
                var userInfoJson = HttpContext.Session.GetString("UserInfo");
                if (!string.IsNullOrEmpty(userInfoJson))
                {
                    var userInfo = JsonSerializer.Deserialize<UserInfoDto>(userInfoJson);
                    ViewBag.UserInfo = userInfo;
                }
            }

            return View();
        }

        public async Task<IActionResult> Profile()
        {
            try
            {
                // Obtener información básica del usuario desde la sesión para el ID
                var userInfoJson = HttpContext.Session.GetString("UserInfo");
                if (string.IsNullOrEmpty(userInfoJson))
                {
                    return RedirectToAction("Login", "Account");
                }

                var userInfo = JsonSerializer.Deserialize<UserInfoDto>(userInfoJson);
                if (userInfo == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                // Obtener perfil completo desde la API
                var profileResponse = await _accountApiService.GetUserProfileByIdAsync(userInfo.Id);
                
                if (profileResponse?.Success == true && profileResponse.Data != null)
                {
                    ViewBag.UserProfile = profileResponse.Data;
                    ViewBag.UserInfo = userInfo; // Para información básica como rol
                }
                else
                {
                    // Si no se puede obtener el perfil completo, usar datos básicos
                    ViewBag.UserInfo = userInfo;
                    ViewBag.ErrorMessage = "No se pudo cargar el perfil completo desde el servidor.";
                }

                // Obtener aplicaciones/actividades del usuario
                try
                {
                    var applicationsResponse = await _volunteerApiService.GetMyApplicationsAsync();
                    if (applicationsResponse?.Success == true && applicationsResponse.Data != null)
                    {
                        ViewBag.UserApplications = applicationsResponse.Data;
                    }

                    // Obtener actividades recientes para la sección de historial
                    var recentActivitiesResponse = await _dashboardApiService.GetRecentActivitiesAsync(5);
                    if (recentActivitiesResponse?.Success == true && recentActivitiesResponse.Data != null)
                    {
                        ViewBag.RecentActivities = recentActivitiesResponse.Data;
                    }

                    // Obtener los eventos del usuario (oportunidades a las que aplicó) para la sección "Mis eventos"
                    var userEventsResponse = await _dashboardApiService.GetUserEventsAsync();
                    if (userEventsResponse?.Success == true && userEventsResponse.Data != null)
                    {
                        ViewBag.UserEvents = userEventsResponse.Data;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error al obtener aplicaciones del usuario");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener perfil del usuario");
                ViewBag.ErrorMessage = "Error de conexión al cargar el perfil.";
                
                // Fallback a datos de sesión
                var userInfoJson = HttpContext.Session.GetString("UserInfo");
                if (!string.IsNullOrEmpty(userInfoJson))
                {
                    var userInfo = JsonSerializer.Deserialize<UserInfoDto>(userInfoJson);
                    ViewBag.UserInfo = userInfo;
                }
            }

            return View();
        }

        public async Task<IActionResult> ProfileONG()
        {
            try
            {
                // Obtener información básica del usuario desde la sesión
                var userInfoJson = HttpContext.Session.GetString("UserInfo");
                if (string.IsNullOrEmpty(userInfoJson))
                {
                    return RedirectToAction("Login", "Account");
                }

                var userInfo = JsonSerializer.Deserialize<UserInfoDto>(userInfoJson);
                if (userInfo == null || userInfo.Organizacion == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                // Obtener perfil completo de la organización desde la API
                var orgProfileResponse = await _accountApiService.GetOrganizationProfileAsync(userInfo.Organizacion.Id);
                
                // Obtener estadísticas de la organización
                _logger.LogInformation("Fetching organization stats...");
                var orgStatsResponse = await _dashboardApiService.GetOrganizationStatsAsync();
                _logger.LogInformation("Organization stats response: Success={Success}, Data={Data}", 
                    orgStatsResponse?.Success, orgStatsResponse?.Data != null ? "Not null" : "Null");
                
                // Obtener eventos de la organización
                _logger.LogInformation("Fetching organization events...");
                var orgEventsResponse = await _dashboardApiService.GetOrganizationEventsAsync();
                _logger.LogInformation("Organization events response: Success={Success}, Data={Data}", 
                    orgEventsResponse?.Success, orgEventsResponse?.Data != null ? "Not null" : "Null");
                
                if (orgProfileResponse?.Success == true && orgProfileResponse.Data != null)
                {
                    ViewBag.OrganizationProfile = orgProfileResponse.Data;
                    ViewBag.UserInfo = userInfo; // Para información básica
                }
                else
                {
                    // Si no se puede obtener el perfil completo, usar datos básicos
                    ViewBag.UserInfo = userInfo;
                    ViewBag.ErrorMessage = "No se pudo cargar el perfil completo de la organización desde el servidor.";
                }

                // Pasar estadísticas y eventos al ViewBag
                if (orgStatsResponse?.Success == true && orgStatsResponse.Data != null)
                {
                    ViewBag.OrganizationStats = orgStatsResponse.Data;
                }

                if (orgEventsResponse?.Success == true && orgEventsResponse.Data != null)
                {
                    ViewBag.OrganizationEvents = orgEventsResponse.Data;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener perfil de la organización");
                ViewBag.ErrorMessage = "Error de conexión al cargar el perfil de la organización.";
                
                // Fallback a datos de sesión
                var userInfoJson = HttpContext.Session.GetString("UserInfo");
                if (!string.IsNullOrEmpty(userInfoJson))
                {
                    var userInfo = JsonSerializer.Deserialize<UserInfoDto>(userInfoJson);
                    ViewBag.UserInfo = userInfo;
                }
            }

            return View();
        }

        public async Task<IActionResult> Reportes(int page = 1, int pageSize = 5, string? search = null)
        {
            try
            {
                // Get user info from session to validate permissions
                var userInfoJson = HttpContext.Session.GetString("UserInfo");
                if (string.IsNullOrEmpty(userInfoJson))
                {
                    return RedirectToAction("Login", "Account");
                }

                var userInfo = System.Text.Json.JsonSerializer.Deserialize<UserInfoDto>(userInfoJson);
                if (userInfo == null || userInfo.Rol != UserRole.Administrador)
                {
                    TempData["MensajeError"] = "No tienes permisos para acceder a esta página.";
                    return RedirectToAction("Index");
                }

                ViewBag.UserInfo = userInfo;

                // Get admin statistics
                try
                {
                    var statsResponse = await _dashboardApiService.GetAdminStatsForReportsAsync();
                    if (statsResponse?.Success == true && statsResponse.Data != null)
                    {
                        ViewBag.AdminStats = statsResponse.Data;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error al obtener estadísticas administrativas");
                }

                // Get volunteers list with pagination
                try
                {
                    var volunteersResponse = await _dashboardApiService.GetVolunteersForReportsAsync(page, pageSize, search);
                    if (volunteersResponse?.Success == true && volunteersResponse.Data != null)
                    {
                        ViewBag.VolunteersData = volunteersResponse.Data;
                        ViewBag.CurrentPage = page;
                        ViewBag.PageSize = pageSize;
                        ViewBag.SearchQuery = search;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error al obtener lista de voluntarios");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en la página de reportes");
                ViewBag.ErrorMessage = "Error al cargar los reportes.";
            }

            return View();
        }

        public async Task<IActionResult> VolunteerDetails(int id)
        {
            ViewData["VolunteerId"] = id;
            
            try
            {
                // Obtener perfil del voluntario específico
                var profileResponse = await _accountApiService.GetUserProfileByIdAsync(id);
                
                if (profileResponse?.Success == true && profileResponse.Data != null)
                {
                    ViewBag.VolunteerProfile = profileResponse.Data;
                }
                else
                {
                    _logger.LogWarning("No se pudo obtener el perfil del voluntario {Id}", id);
                }

                // Obtener aplicaciones/actividades del voluntario
                var applicationsResponse = await _volunteerApiService.GetMyApplicationsAsync();
                
                if (applicationsResponse?.Success == true && applicationsResponse.Data != null)
                {
                    ViewBag.VolunteerApplications = applicationsResponse.Data;
                }
                else
                {
                    _logger.LogWarning("No se pudieron obtener aplicaciones del voluntario {Id}", id);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener detalles del voluntario {Id}", id);
            }

            return View();
        }

        public async Task<IActionResult> VolunteerStats(int id)
        {
            ViewData["VolunteerId"] = id;
            
            try
            {
                // Intentar obtener aplicaciones del voluntario desde la API
                var response = await _volunteerApiService.GetMyApplicationsAsync();
                
                if (response?.Success == true && response.Data != null)
                {
                    ViewBag.VolunteerApplications = response.Data;
                }
                else
                {
                    _logger.LogWarning("No se pudieron obtener aplicaciones del voluntario desde la API");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener estadísticas del voluntario {Id}", id);
            }

            return View();
        }
    }
}
