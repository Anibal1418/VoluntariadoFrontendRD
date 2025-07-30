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
                    TempData["MensajeExito"] = "¡Inicio de sesión exitoso! Bienvenido de vuelta.";
                    TempData["RedirectUrl"] = "/Dashboard/Profile";
                    return View(model);

                    // Intentar llamar a la API primero
                    /*var response = await _accountApiService.LoginAsync(model);
                    
                    if (response?.Success == true && response.Data != null)
                    {
                        // Almacenar token en sesión/cookie
                        HttpContext.Session.SetString("JWTToken", response.Data.Token);
                        HttpContext.Session.SetString("UserInfo", System.Text.Json.JsonSerializer.Serialize(response.Data.User));
                        
                        TempData["MensajeExito"] = "¡Inicio de sesión exitoso! Bienvenido de vuelta.";
                        TempData["RedirectUrl"] = "/Dashboard/Profile";
                        return View(model);
                    }
                    else
                    {
                        // Si la API falla, mostrar error
                        TempData["MensajeError"] = "Credenciales inválidas. " + (response?.Message ?? "Por favor, verifica tu email y contraseña.");
                        return View(model);
                    }*/
                }
                catch (Exception ex)
                {
                    // Si la API no está disponible, mostrar error
                    TempData["MensajeError"] = "Error de conexión. " + ex.Message + " Por favor, inténtalo de nuevo más tarde.";
                    return View(model);
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
                    TempData["MensajeExito"] = "¡Registro exitoso! Tu cuenta ha sido creada correctamente. Ya puedes iniciar sesión.";
                    TempData["RedirectUrl"] = "/Account/Login";
                    return View(model);

                    // PENDIENTE Intentar llamar a la API primero, poner cuando haya API
                    //var response = await _accountApiService.RegisterVolunteerAsync(model);
                    
                    /*if (response?.Success == true)
                    {
                        TempData["MensajeExito"] = "¡Registro exitoso! Tu cuenta ha sido creada correctamente. Ya puedes iniciar sesión.";
                        TempData["RedirectUrl"] = "/Account/Login";
                        return View(model);
                    }
                    else
                    {
                        // Si la API falla, mostrar error
                        TempData["MensajeError"] = "Error en el registro. " + (response?.Message ?? "Por favor, verifica los datos e inténtalo de nuevo.");
                        return View(model);
                    }
                    */
                }
                catch (Exception ex)
                {
                    // Si la API no está disponible, mostrar error
                    TempData["MensajeError"] = "Error de conexión. " + ex.Message + " Por favor, inténtalo de nuevo más tarde.";
                    return View(model);
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
                    TempData["MensajeExito"] = "¡Registro de ONG exitoso! Tu organización ha sido registrada correctamente.";
                        TempData["RedirectUrl"] = "/Account/Login";
                        return View(model);// Intentar llamar a la API primero
                    //var response = await _accountApiService.RegisterONGAsync(model);
                    /*
                    if (response?.Success == true)
                    {
                        TempData["MensajeExito"] = "¡Registro de ONG exitoso! Tu organización ha sido registrada correctamente.";
                        TempData["RedirectUrl"] = "/Account/Login";
                        return View(model);
                    }
                    else
                    {
                        // Si la API falla, mostrar error
                        TempData["MensajeError"] = "Error en el registro de la ONG. " + (response?.Message ?? "Por favor, verifica los datos e inténtalo de nuevo.");
                        return View(model);
                    }
                    */
                }
                catch (Exception ex)
                {
                    // Si la API no está disponible, mostrar error
                    TempData["MensajeError"] = "Error de conexión. " + ex.Message + " Por favor, inténtalo de nuevo más tarde.";
                    return View(model);
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult EditarONG(int id)
        {
            // TODO: Obtener datos de la ONG desde la API
            // Por ahora, creamos un modelo de ejemplo
            var model = new VoluntariosConectadosRD.Models.EditarONGViewModel
            {
                Id = id,
                NombreONG = "Fundación Esperanza Viva",
                RNC = "123456789",
                Telefono = "(809) 123-4567",
                Email = "contacto@fundacionesperanza.com",
                Direccion = "Calle Principal #123",
                Ciudad = "Santo Domingo",
                Provincia = "Distrito Nacional",
                Descripcion = "Dedicados a mejorar la calidad de vida de comunidades vulnerables",
                Sector = "Desarrollo social",
                LogoActual = "~/images/companylogo.jpg"
            };
            
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditarONG(VoluntariosConectadosRD.Models.EditarONGViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // TODO: Implementar llamada a la API para actualizar ONG
                    // var response = await _accountApiService.UpdateONGAsync(model);
                    
                    // Por ahora, simulamos éxito
                    TempData["MensajeExito"] = "¡Información de la ONG actualizada exitosamente!";
                    TempData["RedirectUrl"] = "/Dashboard/ProfileONG";
                    return View(model);
                }
                catch (Exception ex)
                {
                    // Si la API no está disponible, mostrar error
                    TempData["MensajeError"] = "Error de conexión. " + ex.Message + " Por favor, inténtalo de nuevo más tarde.";
                    return View(model);
                }
            }
            return View(model);
        }


    }
} 
