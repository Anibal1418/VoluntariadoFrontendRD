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
                    // Llamar a la API
                    var response = await _accountApiService.LoginAsync(model);
                    
                    if (response?.Success == true && response.Data != null)
                    {
                        // Almacenar token en sesión
                        HttpContext.Session.SetString("JWTToken", response.Data.Token);
                        HttpContext.Session.SetString("UserInfo", System.Text.Json.JsonSerializer.Serialize(response.Data.User));
                        
                        // Configurar header de autorización para futuras llamadas a la API
                        _logger.LogInformation("Usuario {Email} inició sesión exitosamente", model.Email);
                        
                        TempData["MensajeExito"] = "¡Inicio de sesión exitoso! Bienvenido de vuelta.";
                        TempData["RedirectUrl"] = "/Dashboard/Profile";
                        return View(model);
                    }
                    else
                    {
                        // La llamada a la API falló, mostrar error
                        TempData["MensajeError"] = "Credenciales inválidas. " + (response?.Message ?? "Por favor, verifica tu email y contraseña.");
                        return View(model);
                    }
                }
                catch (Exception ex)
                {
                    // API no disponible, mostrar error
                    _logger.LogError(ex, "Error durante el login para {Email}", model.Email);
                    TempData["MensajeError"] = "Error de conexión. Por favor, inténtalo de nuevo más tarde.";
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
                    // Llamar a la API para registrar voluntario
                    var response = await _accountApiService.RegisterVolunteerAsync(model);
                    
                    if (response?.Success == true)
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
                }
                catch (Exception ex)
                {
                    // Si la API no está disponible, mostrar error
                    _logger.LogError(ex, "Error durante el registro de voluntario para {Email}", model.Email);
                    TempData["MensajeError"] = "Error de conexión. Por favor, inténtalo de nuevo más tarde.";
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
                    // Llamar a la API para registrar organización
                    var response = await _accountApiService.RegisterONGAsync(model);
                    
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
                }
                catch (Exception ex)
                {
                    // Si la API no está disponible, mostrar error
                    _logger.LogError(ex, "Error durante el registro de ONG para {Email}", model.Email);
                    TempData["MensajeError"] = "Error de conexión. Por favor, inténtalo de nuevo más tarde.";
                    return View(model);
                }
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditarONG(int id)
        {
            try
            {
                // Obtener datos de la organización desde la API
                var response = await _accountApiService.GetOrganizationProfileAsync(id);
                
                if (response?.Success == true && response.Data != null)
                {
                    var profile = response.Data;
                    var model = new VoluntariosConectadosRD.Models.EditarONGViewModel
                    {
                        Id = profile.Id,
                        NombreONG = profile.Nombre,
                        RNC = profile.NumeroRegistro,
                        Telefono = profile.Telefono,
                        Email = profile.Email,
                        Direccion = profile.Direccion,
                        Descripcion = profile.Descripcion,
                        Sector = profile.TipoOrganizacion,
                        LogoActual = profile.LogoUrl
                    };
                    
                    return View(model);
                }
                else
                {
                    ViewBag.ErrorMessage = "No se pudo cargar el perfil de la organización.";
                    return View(new VoluntariosConectadosRD.Models.EditarONGViewModel { Id = id });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener datos de la organización {Id}", id);
                ViewBag.ErrorMessage = "Error de conexión al cargar los datos de la organización.";
                return View(new VoluntariosConectadosRD.Models.EditarONGViewModel { Id = id });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditarONG(VoluntariosConectadosRD.Models.EditarONGViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Llamar a la API para actualizar ONG
                    var response = await _accountApiService.UpdateONGAsync(model);
                    
                    if (response?.Success == true)
                    {
                        // Respuesta JSON para notificación no invasiva
                        return Json(new { 
                            success = true, 
                            message = response.Message ?? "¡Información de la organización actualizada exitosamente!",
                            redirectUrl = "/Dashboard/ProfileONG"
                        });
                    }
                    else
                    {
                        // Error de la API
                        return Json(new { 
                            success = false, 
                            message = response?.Message ?? "Error al actualizar el perfil de la organización. Por favor, verifica los datos e inténtalo de nuevo."
                        });
                    }
                }
                catch (Exception ex)
                {
                    // Error de conexión
                    _logger.LogError(ex, "Error durante la actualización de ONG para ID {Id}", model.Id);
                    return Json(new { 
                        success = false, 
                        message = "Error de conexión. Por favor, inténtalo de nuevo más tarde."
                    });
                }
            }
            
            // Errores de validación
            var errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList();
            return Json(new { 
                success = false, 
                message = "Por favor, corrige los errores en el formulario.",
                errors = errors
            });
        }

        [HttpGet]
        public async Task<IActionResult> EditarVoluntario(int id)
        {
            try
            {
                // Obtener datos del voluntario desde la API
                var response = await _accountApiService.GetUserProfileByIdAsync(id);
                
                if (response?.Success == true && response.Data != null)
                {
                    var profile = response.Data;
                    var model = new VoluntariosConectadosRD.Models.EditarVoluntarioViewModel
                    {
                        Id = profile.Id,
                        Nombre = profile.Nombre,
                        Apellidos = profile.Apellido,
                        Email = profile.Email,
                        Telefono = profile.Telefono,
                        FechaNacimiento = profile.FechaNacimiento ?? DateTime.Now.AddYears(-18),
                        Descripcion = profile.Biografia,
                        Disponibilidad = profile.Disponibilidad,
                        FotoActual = profile.ImagenUrl
                    };
                    
                    return View(model);
                }
                else
                {
                    ViewBag.ErrorMessage = "No se pudo cargar el perfil del usuario.";
                    return View(new VoluntariosConectadosRD.Models.EditarVoluntarioViewModel { Id = id });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener datos del voluntario {Id}", id);
                ViewBag.ErrorMessage = "Error de conexión al cargar los datos del usuario.";
                return View(new VoluntariosConectadosRD.Models.EditarVoluntarioViewModel { Id = id });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditarVoluntario(VoluntariosConectadosRD.Models.EditarVoluntarioViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Llamar a la API para actualizar voluntario
                    var response = await _accountApiService.UpdateVolunteerAsync(model);
                    
                    if (response?.Success == true)
                    {
                        // Respuesta JSON para notificación no invasiva
                        return Json(new { 
                            success = true, 
                            message = response.Message ?? "¡Información del voluntario actualizada exitosamente!",
                            redirectUrl = "/Dashboard/Profile"
                        });
                    }
                    else
                    {
                        // Error de la API
                        return Json(new { 
                            success = false, 
                            message = response?.Message ?? "Error al actualizar el perfil. Por favor, verifica los datos e inténtalo de nuevo."
                        });
                    }
                }
                catch (Exception ex)
                {
                    // Error de conexión
                    _logger.LogError(ex, "Error durante la actualización de voluntario para ID {Id}", model.Id);
                    return Json(new { 
                        success = false, 
                        message = "Error de conexión. Por favor, inténtalo de nuevo más tarde."
                    });
                }
            }
            
            // Errores de validación
            var errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList();
            return Json(new { 
                success = false, 
                message = "Por favor, corrige los errores en el formulario.",
                errors = errors
            });
        }


    }
} 
