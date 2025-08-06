using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.IO;
using VoluntariosConectadosRD.Models;

namespace VoluntariosConectadosRD.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private const int MaxFileSizeMB = 5; // 5MB máximo
        private readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png" };
        private readonly string[] AllowedMimeTypes = { 
            "image/jpeg", 
            "image/jpg", 
            "image/png",
            "image/jpeg; charset=binary",
            "image/jpg; charset=binary",
            "image/png; charset=binary"
        };

        public DashboardController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Profile()
        {
            var userRole = HttpContext.Session.GetString("UserRole") ?? "Voluntario";
            
            switch (userRole)
            {
                case "Voluntario":
                    return View("Profile");
                case "ONG":
                    return View("ProfileONG");
                case "Admin":
                    return View("ProfileAdmin");
                default:
                    return View("Profile");
            }
        }

        public IActionResult ProfileONG()
        {
            return View();
        }

        public IActionResult ProfileAdmin()
        {
            return View();
        }

        public IActionResult Reportes()
        {
            return View();
        }

        public IActionResult VolunteerStats(int id)
        {
            ViewData["VolunteerId"] = id;
            return View();
        }

        public IActionResult TestNotifications()
        {
            return View();
        }

        public IActionResult VolunteerDetails(string name)
        {
            ViewData["VolunteerName"] = name;
            return View();
        }

        public IActionResult VoluntariosVistas()
        {
            var userRole = HttpContext.Session.GetString("UserRole") ?? "Voluntario";
            
            if (userRole == "Admin")
            {
                return RedirectToAction("VoluntariosAdmin");
            }
            
            return View();
        }

        public IActionResult VoluntariosAdmin()
        {
            var userRole = HttpContext.Session.GetString("UserRole") ?? "Voluntario";
            
            if (userRole != "Admin")
            {
                return RedirectToAction("Profile");
            }
            
            return View();
        }

        public IActionResult ONGAdmin()
        {
            var userRole = HttpContext.Session.GetString("UserRole") ?? "Voluntario";
            
            if (userRole != "Admin")
            {
                return RedirectToAction("Profile");
            }
            
            return View();
        }

        public IActionResult Inicio()
        {
            var userRole = HttpContext.Session.GetString("UserRole") ?? "Voluntario";
            
            switch (userRole)
            {
                case "Voluntario":
                    return RedirectToAction("List", "Events");
                case "ONG":
                    return RedirectToAction("ListONG", "Events");
                case "Admin":
                    return RedirectToAction("ListAdmin", "Events");
                default:
                    return RedirectToAction("List", "Events");
            }
        }

        [HttpPost]
        public async Task<IActionResult> UploadProfileImage(IFormFile imageFile)
        {
            try
            {
                if (imageFile == null || imageFile.Length == 0)
                {
                    return Json(new { success = false, message = "No se seleccionó ningún archivo." });
                }

                // Validar extensión
                var extension = Path.GetExtension(imageFile.FileName).ToLowerInvariant();
                if (!AllowedExtensions.Contains(extension))
                {
                    return Json(new { success = false, message = "Solo se permiten archivos .jpg, .jpeg y .png." });
                }

                // Validar tipo MIME (más flexible)
                var contentType = imageFile.ContentType.ToLowerInvariant();
                var isValidMimeType = AllowedMimeTypes.Any(mime => 
                    contentType.Contains(mime.Replace("; charset=binary", "")) || 
                    contentType == mime);
                
                if (!isValidMimeType)
                {
                    // Si el MIME type no coincide, verificar por extensión
                    var expectedMimeType = extension switch
                    {
                        ".jpg" or ".jpeg" => "image/jpeg",
                        ".png" => "image/png",
                        _ => ""
                    };
                    
                    if (!string.IsNullOrEmpty(expectedMimeType))
                    {
                        // Permitir archivos con extensión correcta aunque el MIME type sea diferente
                        // Esto es común con algunos editores de imagen
                    }
                    else
                    {
                        return Json(new { success = false, message = "Tipo de archivo no válido. Solo se permiten imágenes JPG, JPEG y PNG." });
                    }
                }

                // Validar tamaño (5MB máximo)
                if (imageFile.Length > MaxFileSizeMB * 1024 * 1024)
                {
                    return Json(new { success = false, message = $"El archivo no puede ser mayor a {MaxFileSizeMB}MB." });
                }

                // Crear directorio temporal si no existe
                var webRootPath = _webHostEnvironment.WebRootPath;
                if (string.IsNullOrEmpty(webRootPath))
                {
                    return Json(new { success = false, message = "Error de configuración del servidor." });
                }
                
                var tempUploadPath = Path.Combine(webRootPath, "uploads", "temp");
                if (!Directory.Exists(tempUploadPath))
                {
                    Directory.CreateDirectory(tempUploadPath);
                }

                // Generar nombre único para el archivo temporal
                var fileName = $"temp_{Guid.NewGuid()}{extension}";
                var filePath = Path.Combine(tempUploadPath, fileName);

                // Guardar archivo temporal
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                // Retornar información del archivo temporal
                return Json(new { 
                    success = true, 
                    message = "Imagen subida exitosamente.",
                    tempFileName = fileName,
                    originalName = imageFile.FileName
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error al procesar la imagen: " + ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> SaveProfileImage(string tempFileName, string originalName)
        {
            try
            {
                var webRootPath = _webHostEnvironment.WebRootPath;
                if (string.IsNullOrEmpty(webRootPath))
                {
                    return Json(new { success = false, message = "Error de configuración del servidor." });
                }
                
                var tempFilePath = Path.Combine(webRootPath, "uploads", "temp", tempFileName);
                
                if (!System.IO.File.Exists(tempFilePath))
                {
                    return Json(new { success = false, message = "Archivo temporal no encontrado." });
                }

                // Crear directorio de imágenes de perfil si no existe
                var profileImagesPath = Path.Combine(webRootPath, "uploads", "profiles");
                if (!Directory.Exists(profileImagesPath))
                {
                    Directory.CreateDirectory(profileImagesPath);
                }

                // Generar nombre final para la imagen
                var extension = Path.GetExtension(originalName);
                var finalFileName = $"profile_{Guid.NewGuid()}{extension}";
                var finalFilePath = Path.Combine(profileImagesPath, finalFileName);

                // Mover archivo de temporal a final
                await Task.Run(() => System.IO.File.Move(tempFilePath, finalFilePath));

                // Aquí normalmente guardarías la ruta en la base de datos
                // Por ahora solo retornamos la ruta
                var imageUrl = $"/uploads/profiles/{finalFileName}";

                return Json(new { 
                    success = true, 
                    message = "Imagen guardada exitosamente.",
                    imageUrl = imageUrl
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error al guardar la imagen: " + ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteTempImage(string tempFileName)
        {
            try
            {
                var webRootPath = _webHostEnvironment.WebRootPath;
                if (string.IsNullOrEmpty(webRootPath))
                {
                    return Json(new { success = false, message = "Error de configuración del servidor." });
                }
                
                var tempFilePath = Path.Combine(webRootPath, "uploads", "temp", tempFileName);
                
                if (System.IO.File.Exists(tempFilePath))
                {
                    await Task.Run(() => System.IO.File.Delete(tempFilePath));
                }

                return Json(new { success = true, message = "Archivo temporal eliminado." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error al eliminar archivo: " + ex.Message });
            }
        }
    }
}
