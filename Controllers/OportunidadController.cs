using Microsoft.AspNetCore.Mvc;
using VoluntariosConectadosRD.Models;
using VoluntariosConectadosRD.Services;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System;

namespace VoluntariosConectadosRD.Controllers
{
    public class OportunidadesController : Controller
    {
        private readonly IOportunidadApiService _service;

        public OportunidadesController(IOportunidadApiService service)
        {
            _service = service;
        }

        // Método Index con paginación
        public async Task<IActionResult> Index(int page = 1, int pageSize = 5)
        {
            var todas = await _service.GetAllAsync();
            var paginadas = todas.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            ViewBag.TotalPages = (int)Math.Ceiling((double)todas.Count / pageSize);
            ViewBag.CurrentPage = page;
            return View(paginadas);
        }

        // Método para filtrar por ubicación y fecha
        public async Task<IActionResult> Filtrar(string ubicacion, DateTime? fecha)
        {
            var todas = await _service.GetAllAsync();
            var filtradas = todas.Where(o =>
                (string.IsNullOrEmpty(ubicacion) || o.Ubicacion.Contains(ubicacion, StringComparison.OrdinalIgnoreCase)) &&
                (!fecha.HasValue || o.Fecha.Date == fecha.Value.Date)
            ).ToList();

            // Para reiniciar la paginación
            ViewBag.TotalPages = 1;
            ViewBag.CurrentPage = 1;

            return View("Index", filtradas);
        }

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(Oportunidad oportunidad)
        {
            if (ModelState.IsValid)
            {
                await _service.CreateAsync(oportunidad);
                return RedirectToAction("Index");
            }
            return View(oportunidad);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var oportunidad = await _service.GetByIdAsync(id);
            if (oportunidad == null)
                return NotFound();

            return View(oportunidad);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Oportunidad oportunidad)
        {
            if (ModelState.IsValid)
            {
                await _service.UpdateAsync(oportunidad);
                return RedirectToAction("Index");
            }
            return View(oportunidad);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var oportunidad = await _service.GetByIdAsync(id);
            if (oportunidad == null)
                return NotFound();

            return View(oportunidad);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _service.DeleteAsync(id);
            return RedirectToAction("Index");
        }
    }
}

