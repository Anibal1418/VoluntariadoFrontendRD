using Microsoft.AspNetCore.Mvc;
using VoluntariosConectadosRD.Models;
using VoluntariosConectadosRD.Models.DTOs;
using VoluntariosConectadosRD.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace VoluntariosConectadosRD.Controllers
{
    public class EventsController : Controller
    {
        private readonly IWebHostEnvironment _environment;
        private readonly IVolunteerApiService _volunteerApiService;
        private readonly ILogger<EventsController> _logger;
        private static List<Events> _eventos = new List<Events>();
        private static int _nextId = 1;

        public EventsController(IWebHostEnvironment environment, IVolunteerApiService volunteerApiService, ILogger<EventsController> logger)
        {
            _environment = environment;
            _volunteerApiService = volunteerApiService;
            _logger = logger;

            // Example data initialization
            if (_eventos.Count == 0)
            {
                _eventos.Add(new Events
                {
                    Id = _nextId++,
                    Nombre = "Reforestación en Jarabacoa",
                    Fecha = DateTime.Today.AddDays(5),
                    Ubicacion = "Jarabacoa",
                    Descripcion = "Ayuda a reforestar zonas afectadas por la tala.",
                    Organizador = "Fundación Verde",
                    ImagenUrl = "/images/eventos/reforestacion.jpg",
                    ImagenNombre = "reforestacion.jpg"
                });

                _eventos.Add(new Events
                {
                    Id = _nextId++,
                    Nombre = "Donación de ropa",
                    Fecha = DateTime.Today.AddDays(10),
                    Ubicacion = "Santo Domingo",
                    Descripcion = "Entrega de ropa a comunidades vulnerables.",
                    Organizador = "Caritas Dominicana",
                    ImagenUrl = "/images/eventos/donacion-ropa.jpg",
                    ImagenNombre = "donacion-ropa.jpg"
                });
            }
        }

        public async Task<IActionResult> List()
        {
            try
            {
                // Intentar obtener oportunidades de la API
                var response = await _volunteerApiService.GetVolunteerOpportunitiesAsync();
                
                if (response?.Success == true && response.Data != null)
                {
                    // Convertir oportunidades de API a modelo de eventos para la vista
                    var eventosFromApi = response.Data.Select(opp => new Events
                    {
                        Id = opp.Id,
                        Nombre = opp.Titulo,
                        Fecha = opp.FechaInicio,
                        Ubicacion = opp.Ubicacion,
                        Descripcion = opp.Descripcion,
                        Organizador = opp.Organizacion?.Nombre ?? "Sin especificar",
                        ImagenUrl = "/images/eventos/default.jpg", // Imagen por defecto
                        ImagenNombre = "default.jpg"
                    }).ToList();
                    
                    return View(eventosFromApi.OrderByDescending(e => e.Fecha).ToList());
                }
                else
                {
                    // Si la API falla, usar datos locales como fallback
                    _logger.LogWarning("No se pudieron obtener oportunidades de la API, usando datos locales");
                    var eventosOrdenados = _eventos.OrderByDescending(e => e.Fecha).ToList();
                    return View(eventosOrdenados);
                }
            }
            catch (Exception ex)
            {
                // Si hay error, usar datos locales como fallback
                _logger.LogError(ex, "Error al obtener oportunidades de voluntariado");
                var eventosOrdenados = _eventos.OrderByDescending(e => e.Fecha).ToList();
                return View(eventosOrdenados);
            }
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            try
            {
                // Intentar obtener detalles de la API
                var response = await _volunteerApiService.GetOpportunityDetailsAsync(id.Value);
                
                if (response?.Success == true && response.Data != null)
                {
                    // Convertir detalles de API a modelo de evento para la vista
                    var eventoFromApi = new Events
                    {
                        Id = response.Data.Id,
                        Nombre = response.Data.Titulo,
                        Fecha = response.Data.FechaInicio,
                        Ubicacion = response.Data.Ubicacion,
                        Descripcion = response.Data.Descripcion,
                        Organizador = response.Data.Organizacion?.Nombre ?? "Sin especificar",
                        ImagenUrl = "/images/eventos/default.jpg", // Imagen por defecto
                        ImagenNombre = "default.jpg"
                    };
                    
                    return View(eventoFromApi);
                }
                else
                {
                    // Si la API falla, buscar en datos locales
                    var evento = _eventos.FirstOrDefault(e => e.Id == id);
                    if (evento == null)
                        return NotFound();
                    
                    return View(evento);
                }
            }
            catch (Exception ex)
            {
                // Si hay error, buscar en datos locales
                _logger.LogError(ex, "Error al obtener detalles de la oportunidad {Id}", id);
                var evento = _eventos.FirstOrDefault(e => e.Id == id);
                if (evento == null)
                    return NotFound();
                
                return View(evento);
            }
        }

        public IActionResult Create()
        {
            // Verificar si el usuario está autenticado
            var userInfoJson = HttpContext.Session.GetString("UserInfo");
            if (string.IsNullOrEmpty(userInfoJson))
            {
                TempData["MensajeError"] = "Debes iniciar sesión para crear una oportunidad.";
                return RedirectToAction("Login", "Account");
            }

            var userInfo = System.Text.Json.JsonSerializer.Deserialize<VoluntariosConectadosRD.Models.DTOs.UserInfoDto>(userInfoJson);
            
            // Verificar si el usuario es una organización
            if (userInfo?.Rol != VoluntariosConectadosRD.Models.DTOs.UserRole.Organizacion)
            {
                TempData["MensajeError"] = "Solo las organizaciones pueden crear oportunidades de voluntariado.";
                return RedirectToAction("List");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Events evento, IFormFile imagenArchivo)
        {
            // Verificar autenticación y rol antes de procesar
            var userInfoJson = HttpContext.Session.GetString("UserInfo");
            if (string.IsNullOrEmpty(userInfoJson))
            {
                return Json(new { 
                    success = false, 
                    message = "Debes iniciar sesión para crear una oportunidad."
                });
            }

            var userInfo = System.Text.Json.JsonSerializer.Deserialize<VoluntariosConectadosRD.Models.DTOs.UserInfoDto>(userInfoJson);
            if (userInfo?.Rol != VoluntariosConectadosRD.Models.DTOs.UserRole.Organizacion)
            {
                return Json(new { 
                    success = false, 
                    message = "Solo las organizaciones pueden crear oportunidades de voluntariado."
                });
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Validación adicional de fecha
                    if (evento.Fecha.Date < DateTime.Today)
                    {
                        return Json(new { 
                            success = false, 
                            message = "La fecha del evento no puede ser anterior a hoy."
                        });
                    }

                    // Establecer valores por defecto para campos opcionales
                    evento.ImagenUrl ??= "/images/eventos/default.jpg";
                    evento.ImagenNombre ??= "default.jpg";

                    // Intentar crear oportunidad a través de la API
                    var createDto = new Models.DTOs.CreateOpportunityDto
                    {
                        Titulo = evento.Nombre,
                        Descripcion = evento.Descripcion ?? "",
                        FechaInicio = evento.Fecha,
                        FechaFin = evento.Fecha.AddHours(8), // Por defecto 8 horas de duración
                        Ubicacion = evento.Ubicacion,
                        DuracionHoras = 8, // Por defecto 8 horas
                        VoluntariosRequeridos = 10 // Valor por defecto
                    };

                    var response = await _volunteerApiService.CreateOpportunityAsync(createDto);
                    
                    if (response?.Success == true)
                    {
                        _logger.LogInformation("Oportunidad creada exitosamente a través de la API");
                        return Json(new { 
                            success = true, 
                            message = "¡Evento creado exitosamente!",
                            redirectUrl = Url.Action(nameof(List))
                        });
                    }
                    else
                    {
                        // Si la API falla, verificar el tipo de error
                        var errorMessage = response?.Message ?? "Error al crear la oportunidad";
                        _logger.LogWarning("Error al crear oportunidad en la API: {Message}", errorMessage);
                        
                        // Si el error es por autorización, retornar mensaje claro
                        if (errorMessage.Contains("Usuario no asociado a una organización") || 
                            errorMessage.Contains("Usuario no válido"))
                        {
                            return Json(new { 
                                success = false, 
                                message = errorMessage
                            });
                        }
                        
                        // Si es otro tipo de error, intentar crear localmente como fallback
                        _logger.LogInformation("Intentando crear oportunidad localmente como fallback");
                        
                        // Manejar subida de imagen
                        if (imagenArchivo != null && imagenArchivo.Length > 0)
                        {
                            var uploadsFolder = Path.Combine(_environment.WebRootPath, "images", "eventos");

                            if (!Directory.Exists(uploadsFolder))
                            {
                                Directory.CreateDirectory(uploadsFolder);
                            }

                            var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(imagenArchivo.FileName);
                            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                            using (var fileStream = new FileStream(filePath, FileMode.Create))
                            {
                                await imagenArchivo.CopyToAsync(fileStream);
                            }

                            evento.ImagenUrl = $"/images/eventos/{uniqueFileName}";
                            evento.ImagenNombre = imagenArchivo.FileName;
                        }

                        evento.Id = _nextId++;
                        _eventos.Add(evento);
                        return Json(new { 
                            success = true, 
                            message = "¡Evento creado exitosamente! (Modo local)",
                            redirectUrl = Url.Action(nameof(List))
                        });
                    }
                }
                catch (Exception ex)
                {
                    // Error general
                    _logger.LogError(ex, "Error al crear evento");
                    return Json(new { 
                        success = false, 
                        message = "Error al crear el evento. Por favor, inténtalo de nuevo."
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Apply(int id, string? message = null)
        {
            try
            {
                // Verificar si el usuario está autenticado
                var token = HttpContext.Session.GetString("JWTToken");
                if (string.IsNullOrEmpty(token))
                {
                    TempData["MensajeError"] = "Debes iniciar sesión para aplicar a una oportunidad.";
                    return RedirectToAction("Login", "Account");
                }

                // Intentar aplicar a la oportunidad a través de la API
                var response = await _volunteerApiService.ApplyToOpportunityAsync(id, message);
                
                if (response?.Success == true)
                {
                    TempData["MensajeExito"] = "¡Aplicación enviada exitosamente! La organización revisará tu solicitud.";
                }
                else
                {
                    TempData["MensajeError"] = "Error al enviar la aplicación. " + (response?.Message ?? "Por favor, inténtalo de nuevo.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al aplicar a la oportunidad {Id}", id);
                TempData["MensajeError"] = "Error de conexión. Por favor, inténtalo de nuevo más tarde.";
            }

            return RedirectToAction("Details", new { id });
        }
    }
}