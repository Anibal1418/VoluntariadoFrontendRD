using Microsoft.AspNetCore.Mvc;
using VoluntariosConectadosRD.Services;
using VoluntariosConectadosRD.Models.DTOs;

namespace VoluntariosConectadosRD.Controllers
{
    public class OrganizationsController : Controller
    {
        private readonly IBaseApiService _baseApiService;
        private readonly ILogger<OrganizationsController> _logger;

        public OrganizationsController(IBaseApiService baseApiService, ILogger<OrganizationsController> logger)
        {
            _baseApiService = baseApiService;
            _logger = logger;
        }

        /// <summary>
        /// Organization details page showing information and description
        /// </summary>
        /// <param name="id">Organization ID</param>
        /// <returns>Organization details view</returns>
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                _logger.LogInformation("Attempting to fetch organization details for ID {Id}", id);
                var response = await _baseApiService.GetAsync<ApiResponseDto<OrganizationProfileDto>>($"profile/organization/{id}");
                
                _logger.LogInformation("API Response - Success: {Success}, Message: {Message}, Data: {HasData}", 
                    response?.Success, response?.Message, response?.Data != null);
                
                if (response?.Success == true && response.Data != null)
                {
                    return View(response.Data);
                }
                else
                {
                    _logger.LogWarning("Organization not found or API returned error. Response: {Response}", response?.Message);
                    TempData["ErrorMessage"] = response?.Message ?? "Organización no encontrada";
                    return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading organization details for ID {Id}", id);
                TempData["ErrorMessage"] = "Error al cargar los detalles de la organización";
                return RedirectToAction("Index", "Home");
            }
        }
    }
}