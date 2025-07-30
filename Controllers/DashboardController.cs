using Microsoft.AspNetCore.Mvc;

namespace VoluntariosConectadosRD.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Profile()
        {
            return View();
        }

        public IActionResult ProfileONG()
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
    }
}
