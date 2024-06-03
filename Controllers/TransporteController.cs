using Microsoft.AspNetCore.Mvc;

namespace temperature_analysis.Controllers
{
    public class TransporteController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.Theme = HttpContext.Session.GetString("Theme");
            ViewBag.UserLogin = HelperController.LoginSessionVerification(HttpContext.Session);
            ViewBag.IsAdmin = HelperController.AdminSessionVerification(HttpContext.Session);
            return View();
        }
    }
}
