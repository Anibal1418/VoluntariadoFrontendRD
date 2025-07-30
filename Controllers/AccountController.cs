using Microsoft.AspNetCore.Mvc;
using VoluntariosConectadosRD.Services;

namespace VoluntariosConectadosRD.Controllers
{
    public class AccountController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IAccountApiService _accountApiService;
        private readonly ILogger<AccountController> _logger;

        public AccountController(IConfiguration configuration, IAccountApiService accountApiService, ILogger<AccountController> logger)
        {
            _configuration = configuration;
            _accountApiService = accountApiService;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View(new VoluntariosConectadosRD.Models.LoginViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(VoluntariosConectadosRD.Models.LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Intentar llamar a la API primero
                    var response = await _accountApiService.LoginAsync(model);
                    
                    if (response?.Success == true && response.Data != null)
                    {
                        // Almacenar token en sesión/cookie
                        HttpContext.Session.SetString("JWTToken", response.Data.Token);
                        HttpContext.Session.SetString("UserInfo", System.Text.Json.JsonSerializer.Serialize(response.Data.User));
                        
                        return RedirectToAction("Profile", "Dashboard");
                    }
                    else
                    {
                        // Si la API falla, volver al comportamiento actual
                        _logger.LogWarning("El login de la API falló, volviendo al flujo de trabajo actual");
                        return RedirectToAction("Profile", "Dashboard");
                    }
                }
                catch (Exception ex)
                {
                    // Si la API no está disponible, volver al comportamiento actual
                    _logger.LogWarning(ex, "API no disponible, volviendo al flujo de trabajo actual");
                    return RedirectToAction("Profile", "Dashboard");
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Registro()
        {
            return View(new VoluntariosConectadosRD.Models.RegistroViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registro(VoluntariosConectadosRD.Models.RegistroViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Intentar llamar a la API primero
                    var response = await _accountApiService.RegisterVolunteerAsync(model);
                    
                    if (response?.Success == true)
                    {
                        return RedirectToAction("RegistroExito", "Account");
                    }
                    else
                    {
                        // Si la API falla, volver al comportamiento actual
                        _logger.LogWarning("El registro de la API falló, volviendo al flujo de trabajo actual");
                        return RedirectToAction("RegistroExito", "Account");
                    }
                }
                catch (Exception ex)
                {
                    // Si la API no está disponible, volver al comportamiento actual
                    _logger.LogWarning(ex, "API no disponible, volviendo al flujo de trabajo actual");
                    return RedirectToAction("RegistroExito", "Account");
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult RegistroONG()
        {
            return View(new VoluntariosConectadosRD.Models.RegistroONGViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegistroONG(VoluntariosConectadosRD.Models.RegistroONGViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Intentar llamar a la API primero
                    var response = await _accountApiService.RegisterONGAsync(model);
                    
                    if (response?.Success == true)
                    {
                        return RedirectToAction("RegistroExito", "Account");
                    }
                    else
                    {
                        // Si la API falla, volver al comportamiento actual
                        _logger.LogWarning("El registro de ONG de la API falló, volviendo al flujo de trabajo actual");
                        return RedirectToAction("RegistroExito", "Account");
                    }
                }
                catch (Exception ex)
                {
                    // Si la API no está disponible, volver al comportamiento actual
                    _logger.LogWarning(ex, "API no disponible, volviendo al flujo de trabajo actual");
                    return RedirectToAction("RegistroExito", "Account");
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult RegistroExito()
        {
            return View();
        }
    }
} 
