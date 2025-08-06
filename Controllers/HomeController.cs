using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using VoluntariosConectadosRD.Models;
using VoluntariosConectadosRD.Services;

namespace VoluntariosConectadosRD.Controllers
{

    //Prueba del docente Omar
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IVolunteerApiService _volunteerApiService;
        private readonly IDashboardApiService _dashboardApiService;

        public HomeController(ILogger<HomeController> logger, IVolunteerApiService volunteerApiService, IDashboardApiService dashboardApiService)
        {
            _logger = logger;
            _volunteerApiService = volunteerApiService;
            _dashboardApiService = dashboardApiService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                // Obtener estadísticas generales de la plataforma (endpoint público)
                var statsResponse = await _dashboardApiService.GetDashboardStatsAsync();
                if (statsResponse?.Success == true && statsResponse.Data != null)
                {
                    ViewBag.PlatformStats = statsResponse.Data;
                }

                // Obtener oportunidades destacadas
                var opportunitiesResponse = await _volunteerApiService.GetVolunteerOpportunitiesAsync();
                if (opportunitiesResponse?.Success == true && opportunitiesResponse.Data != null)
                {
                    ViewBag.FeaturedOpportunities = opportunitiesResponse.Data.Take(3).ToList();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener datos para la página de inicio");
            }

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult ComoFunciona()
        {
            return View();
        }

        public IActionResult Contacto()
        {
            return View();
        }

        public IActionResult Terms()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
