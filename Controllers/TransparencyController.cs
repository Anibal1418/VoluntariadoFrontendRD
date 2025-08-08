using Microsoft.AspNetCore.Mvc;
using VoluntariosConectadosRD.Services;
using VoluntariadoConectadoRD.Models.DTOs;

namespace VoluntariosConectadosRD.Controllers
{
    public class TransparencyController : Controller
    {
        private readonly ITransparencyApiService _transparencyApiService;
        private readonly ILogger<TransparencyController> _logger;

        public TransparencyController(ITransparencyApiService transparencyApiService, ILogger<TransparencyController> logger)
        {
            _transparencyApiService = transparencyApiService;
            _logger = logger;
        }

        public async Task<IActionResult> Index(TransparencyFiltersDto? filters = null)
        {
            try
            {
                var response = await _transparencyApiService.GetOrganizationsFinancialSummaryAsync(filters);
                var organizations = response?.Data ?? new List<OrganizationTransparencyDto>();

                ViewBag.Filters = filters ?? new TransparencyFiltersDto();
                
                // Get filter options
                var yearsResponse = await _transparencyApiService.GetAvailableYearsAsync();
                ViewBag.AvailableYears = yearsResponse?.Data ?? new List<int>();

                var typesResponse = await _transparencyApiService.GetOrganizationTypesAsync();
                ViewBag.OrganizationTypes = typesResponse?.Data ?? new List<string>();

                // Get platform overview
                var overviewResponse = await _transparencyApiService.GetPlatformFinancialOverviewAsync();
                ViewBag.PlatformOverview = overviewResponse?.Data;

                return View(organizations);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading transparency index page");
                TempData["ErrorMessage"] = "Error al cargar la información de transparencia";
                return View(new List<OrganizationTransparencyDto>());
            }
        }

        public async Task<IActionResult> Organization(int id)
        {
            try
            {
                var response = await _transparencyApiService.GetOrganizationFinancialDetailsAsync(id);
                
                if (response?.Data == null)
                {
                    TempData["ErrorMessage"] = "Organización no encontrada o no tiene reportes públicos";
                    return RedirectToAction("Index");
                }

                return View(response.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading organization details for ID {Id}", id);
                TempData["ErrorMessage"] = "Error al cargar los detalles de la organización";
                return RedirectToAction("Index");
            }
        }

        public async Task<IActionResult> Report(int id)
        {
            try
            {
                var response = await _transparencyApiService.GetFinancialReportDetailsAsync(id);
                
                if (response?.Data == null)
                {
                    TempData["ErrorMessage"] = "Reporte financiero no encontrado o no es público";
                    return RedirectToAction("Index");
                }

                return View(response.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading report details for ID {Id}", id);
                TempData["ErrorMessage"] = "Error al cargar los detalles del reporte";
                return RedirectToAction("Index");
            }
        }


        [HttpGet]
        public async Task<IActionResult> GetFilteredOrganizations([FromQuery] TransparencyFiltersDto filters)
        {
            try
            {
                var response = await _transparencyApiService.GetOrganizationsFinancialSummaryAsync(filters);
                var organizations = response?.Data ?? new List<OrganizationTransparencyDto>();
                
                return PartialView("_OrganizationList", organizations);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting filtered organizations");
                return PartialView("_OrganizationList", new List<OrganizationTransparencyDto>());
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetPlatformOverview()
        {
            try
            {
                var response = await _transparencyApiService.GetPlatformFinancialOverviewAsync();
                return Json(response?.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting platform overview");
                return Json(null);
            }
        }
    }
}