using Microsoft.AspNetCore.Mvc;
using VoluntariosConectadosRD.Services;
using VoluntariosConectadosRD.Models.DTOs;
using VoluntariadoConectadoRD.Models.DTOs;

namespace VoluntariosConectadosRD.Controllers
{
    public class SearchController : Controller
    {
        private readonly ILogger<SearchController> _logger;
        private readonly IBaseApiService _baseApiService;

        public SearchController(ILogger<SearchController> logger, IBaseApiService baseApiService)
        {
            _logger = logger;
            _baseApiService = baseApiService;
        }

        [HttpGet]
        public IActionResult Index(string q = "", string type = "opportunities")
        {
            ViewBag.SearchQuery = q;
            ViewBag.SearchType = type;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SearchOpportunities([FromBody] OpportunitySearchDto searchDto)
        {
            try
            {
                var response = await _baseApiService.PostAsync<SearchResultDto<dynamic>>("api/Voluntariado/search/opportunities", searchDto);
                
                return Json(new { 
                    success = response?.Success ?? false, 
                    data = response?.Data,
                    message = response?.Message 
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching opportunities");
                return Json(new { 
                    success = false, 
                    message = "Error de conexión al buscar oportunidades" 
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> SearchVolunteers([FromBody] VolunteerSearchDto searchDto)
        {
            try
            {
                var token = HttpContext.Session.GetString("JWTToken");
                if (string.IsNullOrEmpty(token))
                {
                    return Json(new { success = false, message = "Sesión expirada" });
                }

                _baseApiService.SetAuthToken(token);
                var response = await _baseApiService.PostAsync<SearchResultDto<dynamic>>("api/Voluntariado/search/volunteers", searchDto);
                
                return Json(new { 
                    success = response?.Success ?? false, 
                    data = response?.Data,
                    message = response?.Message 
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching volunteers");
                return Json(new { 
                    success = false, 
                    message = "Error de conexión al buscar voluntarios" 
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> SearchOrganizations([FromBody] OrganizationSearchDto searchDto)
        {
            try
            {
                var response = await _baseApiService.PostAsync<SearchResultDto<dynamic>>("api/Voluntariado/search/organizations", searchDto);
                
                return Json(new { 
                    success = response?.Success ?? false, 
                    data = response?.Data,
                    message = response?.Message 
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching organizations");
                return Json(new { 
                    success = false, 
                    message = "Error de conexión al buscar organizaciones" 
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> QuickSearch([FromBody] QuickSearchDto searchDto)
        {
            try
            {
                var response = await _baseApiService.PostAsync<QuickSearchResultDto>("api/Voluntariado/search/quick", searchDto);
                
                return Json(new { 
                    success = response?.Success ?? false, 
                    data = response?.Data,
                    message = response?.Message 
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error performing quick search");
                return Json(new { 
                    success = false, 
                    message = "Error de conexión al realizar búsqueda rápida" 
                });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetFilters(string type)
        {
            try
            {
                if (string.IsNullOrEmpty(type) || !new[] { "opportunities", "volunteers", "organizations" }.Contains(type.ToLower()))
                {
                    return Json(new { success = false, message = "Tipo de filtro inválido" });
                }

                var response = await _baseApiService.GetAsync<SearchFilters>($"api/Voluntariado/search/filters/{type}");
                
                return Json(new { 
                    success = response?.Success ?? false, 
                    data = response?.Data,
                    message = response?.Message 
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting search filters for type: {Type}", type);
                return Json(new { 
                    success = false, 
                    message = "Error de conexión al obtener filtros" 
                });
            }
        }
    }
}