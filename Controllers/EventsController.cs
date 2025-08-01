using Microsoft.AspNetCore.Mvc;
using VoluntariosConectadosRD.Models;
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
        private static List<Events> _eventos = new List<Events>();
        private static int _nextId = 1;

        public EventsController(IWebHostEnvironment environment)
        {
            _environment = environment;

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
                    ImagenUrl = "/images/eventos/donacion-ropa.jpg",
                    ImagenNombre = "donacion-ropa.jpg"
                });
            }
        }

        public IActionResult List()
        {
            var eventosOrdenados = _eventos.OrderByDescending(e => e.Fecha).ToList();
            return View(eventosOrdenados);
        }

        public IActionResult Details(int? id)
        {
            if (id == null)
                return NotFound();

            var evento = _eventos.FirstOrDefault(e => e.Id == id);

            if (evento == null)
                return NotFound();

            return View(evento);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Events evento, IFormFile imagenArchivo)
        {
            if (ModelState.IsValid)
            {
                // Image upload handling
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
                return RedirectToAction(nameof(List));
            }

            return View(evento);
        }
    }
}