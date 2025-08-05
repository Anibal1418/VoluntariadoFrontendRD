using Microsoft.AspNetCore.Mvc;

namespace VoluntariosConectadosRD.Controllers
{
    public class DashboardController : Controller
    {
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
    }
}
