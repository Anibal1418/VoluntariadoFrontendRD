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

        public EventsController(IWebHostEnvironment environment, IVolunteerApiService volunteerApiService, ILogger<EventsController> logger)
        {
            _environment = environment;
            _volunteerApiService = volunteerApiService;
            _logger = logger;
        }

        public async Task<IActionResult> List(int page = 1, int pageSize = 6, string? search = null)
        {
            try
            {
                // Obtener oportunidades de la API
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
                        Organizador = opp.OrganizacionNombre ?? opp.ONG ?? "Sin especificar",
                        ImagenUrl = "/images/eventos/default.jpg", // Imagen por defecto
                        ImagenNombre = "default.jpg"
                    }).AsQueryable();

                    // Apply search filter if provided
                    if (!string.IsNullOrEmpty(search))
                    {
                        eventosFromApi = eventosFromApi.Where(e => 
                            e.Nombre.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                            e.Ubicacion.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                            e.Organizador.Contains(search, StringComparison.OrdinalIgnoreCase));
                    }

                    // Order and apply pagination
                    var totalCount = eventosFromApi.Count();
                    var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
                    
                    var pagedEvents = eventosFromApi
                        .OrderByDescending(e => e.Fecha)
                        .Skip((page - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

                    // Pass pagination info to view
                    ViewBag.CurrentPage = page;
                    ViewBag.PageSize = pageSize;
                    ViewBag.TotalCount = totalCount;
                    ViewBag.TotalPages = totalPages;
                    ViewBag.SearchQuery = search;
                    ViewBag.HasPreviousPage = page > 1;
                    ViewBag.HasNextPage = page < totalPages;
                    
                    return View(pagedEvents);
                }
                else
                {
                    // Si la API no devuelve datos, mostrar lista vacía
                    _logger.LogWarning("No se pudieron obtener oportunidades de la API");
                    ViewBag.ErrorMessage = "No se pudieron cargar las oportunidades de voluntariado en este momento.";
                    ViewBag.CurrentPage = page;
                    ViewBag.PageSize = pageSize;
                    ViewBag.TotalCount = 0;
                    ViewBag.TotalPages = 0;
                    ViewBag.SearchQuery = search;
                    return View(new List<Events>());
                }
            }
            catch (Exception ex)
            {
                // Si hay error, mostrar lista vacía con mensaje de error
                _logger.LogError(ex, "Error al obtener oportunidades de voluntariado");
                ViewBag.ErrorMessage = "Error de conexión. No se pudieron cargar las oportunidades de voluntariado.";
                ViewBag.CurrentPage = page;
                ViewBag.PageSize = pageSize;
                ViewBag.TotalCount = 0;
                ViewBag.TotalPages = 0;
                ViewBag.SearchQuery = search;
                return View(new List<Events>());
            }
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            try
            {
                // Obtener detalles de la API
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
                    // Si la API no encuentra la oportunidad
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                // Si hay error de conexión
                _logger.LogError(ex, "Error al obtener detalles de la oportunidad {Id}", id);
                ViewBag.ErrorMessage = "Error de conexión. No se pudieron cargar los detalles de la oportunidad.";
                return View("Error");
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
            if (userInfo?.Rol != (int)VoluntariosConectadosRD.Models.DTOs.UserRole.Organizacion)
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
            if (userInfo?.Rol != (int)VoluntariosConectadosRD.Models.DTOs.UserRole.Organizacion)
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

                    // Set category-based default image if no file uploaded
                    if (string.IsNullOrEmpty(evento.ImagenUrl))
                    {
                        evento.ImagenUrl = GetCategoryImage(evento.Descripcion);
                        evento.ImagenNombre = Path.GetFileName(evento.ImagenUrl);
                    }

                    // Intentar crear oportunidad a través de la API
                    var createDto = new VoluntariadoConectadoRD.Models.DTOs.CreateOpportunityDto
                    {
                        Titulo = evento.Nombre,
                        Descripcion = evento.Descripcion ?? "",
                        FechaInicio = evento.Fecha,
                        FechaFin = evento.FechaFin ?? evento.Fecha.AddHours(evento.DuracionHoras),
                        Ubicacion = evento.Ubicacion,
                        DuracionHoras = evento.DuracionHoras,
                        VoluntariosRequeridos = evento.VoluntariosRequeridos
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
                        
                        // Return the API error message
                        _logger.LogError("Error creating event via API: {ErrorMessage}", errorMessage);
                        return Json(new { 
                            success = false, 
                            message = $"Error del servidor: {errorMessage}"
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

        public async Task<IActionResult> ListAdmin(int page = 1, int pageSize = 10)
        {
            try
            {
                // Get all opportunities for admin view
                var response = await _volunteerApiService.GetVolunteerOpportunitiesAsync();
                
                if (response?.Success == true && response.Data != null)
                {
                    var allEvents = response.Data.Select(opp => new Events
                    {
                        Id = opp.Id,
                        Nombre = opp.Titulo,
                        Fecha = opp.FechaInicio,
                        Ubicacion = opp.Ubicacion,
                        Descripcion = opp.Descripcion,
                        Organizador = opp.OrganizacionNombre ?? opp.ONG ?? "Sin especificar",
                        ImagenUrl = GetCategoryImage(opp.Descripcion),
                        ImagenNombre = "default.jpg"
                    }).ToList();

                    // Calculate pagination
                    var totalItems = allEvents.Count;
                    var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
                    var paginatedEvents = allEvents.Skip((page - 1) * pageSize).Take(pageSize).ToList();

                    ViewBag.CurrentPage = page;
                    ViewBag.TotalPages = totalPages;
                    ViewBag.PageSize = pageSize;

                    return View(paginatedEvents);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading admin events list");
            }

            // Return empty list on error
            ViewBag.CurrentPage = 1;
            ViewBag.TotalPages = 1;
            return View(new List<Events>());
        }

        public async Task<IActionResult> ListONG(int page = 1, int pageSize = 10)
        {
            try
            {
                // Get user's organization events
                var userInfoJson = HttpContext.Session.GetString("UserInfo");
                if (!string.IsNullOrEmpty(userInfoJson))
                {
                    var userInfo = System.Text.Json.JsonSerializer.Deserialize<UserInfoDto>(userInfoJson);
                    if (userInfo?.Organizacion != null)
                    {
                        var response = await _volunteerApiService.GetOrganizationOpportunitiesAsync();
                        
                        if (response?.Success == true && response.Data != null)
                        {
                            var orgEvents = response.Data.Select(opp => new Events
                            {
                                Id = opp.Id,
                                Nombre = opp.Titulo,
                                Fecha = opp.FechaInicio,
                                Ubicacion = opp.Ubicacion,
                                Descripcion = opp.Descripcion,
                                Organizador = userInfo.Organizacion.Nombre,
                                ImagenUrl = GetCategoryImage(opp.Descripcion),
                                ImagenNombre = "default.jpg"
                            }).ToList();

                            // Calculate pagination
                            var totalItems = orgEvents.Count;
                            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
                            var paginatedEvents = orgEvents.Skip((page - 1) * pageSize).Take(pageSize).ToList();

                            ViewBag.CurrentPage = page;
                            ViewBag.TotalPages = totalPages;
                            ViewBag.PageSize = pageSize;

                            return View(paginatedEvents);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading organization events list");
            }

            // Return empty list on error
            ViewBag.CurrentPage = 1;
            ViewBag.TotalPages = 1;
            return View(new List<Events>());
        }

        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                // Get opportunity details from API
                var response = await _volunteerApiService.GetOpportunityByIdAsync(id);
                
                if (response?.Success == true && response.Data != null)
                {
                    var opportunity = response.Data;
                    var evento = new Events
                    {
                        Id = opportunity.Id,
                        Nombre = opportunity.Titulo,
                        Descripcion = opportunity.Descripcion,
                        Fecha = opportunity.FechaInicio,
                        FechaFin = opportunity.FechaFin,
                        Ubicacion = opportunity.Ubicacion,
                        DuracionHoras = opportunity.DuracionHoras,
                        VoluntariosRequeridos = opportunity.VoluntariosRequeridos,
                        ImagenUrl = GetCategoryImage(opportunity.Descripcion),
                        Organizador = opportunity.Organizacion?.Nombre ?? "Sin especificar"
                    };

                    return View(evento);
                }
                else
                {
                    TempData["ErrorMessage"] = "No se pudo cargar el evento.";
                    return RedirectToAction("List");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading event for editing: {Id}", id);
                TempData["ErrorMessage"] = "Error al cargar el evento.";
                return RedirectToAction("List");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Events evento)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { 
                        success = false, 
                        message = "Por favor, corrige los errores en el formulario.",
                        errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToArray()
                    });
                }

                // Create update DTO
                var updateDto = new VoluntariadoConectadoRD.Models.DTOs.UpdateOpportunityDto
                {
                    Id = evento.Id,
                    Titulo = evento.Nombre,
                    Descripcion = evento.Descripcion ?? "",
                    FechaInicio = evento.Fecha,
                    FechaFin = evento.FechaFin ?? evento.Fecha.AddHours(evento.DuracionHoras),
                    Ubicacion = evento.Ubicacion,
                    DuracionHoras = evento.DuracionHoras,
                    VoluntariosRequeridos = evento.VoluntariosRequeridos
                };

                var response = await _volunteerApiService.UpdateOpportunityAsync(evento.Id, updateDto);
                
                if (response?.Success == true)
                {
                    return Json(new { 
                        success = true, 
                        message = "¡Evento actualizado exitosamente!",
                        redirectUrl = Url.Action("Details", new { id = evento.Id })
                    });
                }
                else
                {
                    return Json(new { 
                        success = false, 
                        message = response?.Message ?? "Error al actualizar el evento"
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating event: {Id}", evento.Id);
                return Json(new { 
                    success = false, 
                    message = "Error interno del servidor" 
                });
            }
        }

        public async Task<IActionResult> Applicants(int id)
        {
            try
            {
                // Get opportunity details
                var opportunityResponse = await _volunteerApiService.GetOpportunityByIdAsync(id);
                if (opportunityResponse?.Success != true || opportunityResponse.Data == null)
                {
                    TempData["ErrorMessage"] = "No se pudo cargar el evento.";
                    return RedirectToAction("List");
                }

                // Get applications for this opportunity
                var applicationsResponse = await _volunteerApiService.GetApplicationsForOpportunityAsync(id);
                
                ViewBag.Opportunity = opportunityResponse.Data;
                ViewBag.Applications = applicationsResponse?.Data?.Cast<VolunteerApplicationDetailDto>().ToList() ?? new List<VolunteerApplicationDetailDto>();

                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading applicants for event: {Id}", id);
                TempData["ErrorMessage"] = "Error al cargar las aplicaciones.";
                return RedirectToAction("List");
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateApplicationStatus(int applicationId, string status, string? message = "")
        {
            try
            {
                var response = await _volunteerApiService.UpdateApplicationStatusAsync(applicationId, int.Parse(status));
                
                if (response?.Success == true)
                {
                    return Json(new { 
                        success = true, 
                        message = "Estado de la aplicación actualizado exitosamente"
                    });
                }
                else
                {
                    return Json(new { 
                        success = false, 
                        message = response?.Message ?? "Error al actualizar el estado"
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating application status: {ApplicationId}", applicationId);
                return Json(new { 
                    success = false, 
                    message = "Error interno del servidor" 
                });
            }
        }

        private static string GetCategoryImage(string? description)
        {
            if (string.IsNullOrEmpty(description))
                return "/images/eventos/default.jpg";
            
            var desc = description.ToLower();
            
            // Category-based image mapping
            if (desc.Contains("educación") || desc.Contains("educacion") || desc.Contains("enseñanza") || desc.Contains("escuela"))
                return "/images/eventos/educacion.jpg";
            else if (desc.Contains("salud") || desc.Contains("médico") || desc.Contains("hospital") || desc.Contains("medicina"))
                return "/images/eventos/salud.jpg";
            else if (desc.Contains("ambiente") || desc.Contains("naturaleza") || desc.Contains("limpieza") || desc.Contains("reciclaje"))
                return "/images/eventos/medio-ambiente.jpg";
            else if (desc.Contains("comunitario") || desc.Contains("comunidad") || desc.Contains("vecindario"))
                return "/images/eventos/comunitario.jpg";
            else if (desc.Contains("emergencia") || desc.Contains("ayuda") || desc.Contains("desastre") || desc.Contains("socorro"))
                return "/images/eventos/emergencia.jpg";
            else
                return "/images/eventos/default.jpg";
        }
    }
}