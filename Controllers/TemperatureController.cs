using Microsoft.AspNetCore.Mvc;
using temperature_analysis.Models;
using temperature_analysis.DAO;

namespace temperature_analysis.Controllers
{
    public class TemperatureController : Controller
    {
        private readonly TemperatureDAO _temperatureDAO;

        public TemperatureController()
        {
            _temperatureDAO = new TemperatureDAO();
        }

        [Route("temp")]
        public async Task<IActionResult> Get()
        {
            var data = await _temperatureDAO.GetPastData(10);

            // Converting the JSON elements to the ViewModel
            var temperatureData = new List<TemperatureViewModel>();
            foreach (var item in data)
            {
                temperatureData.Add(new TemperatureViewModel
                {
                    Id = item.TryGetProperty("id", out var idProp) ? idProp.GetInt32() : 0,
                    Timestamp = DateTime.Parse(item.GetProperty("recvTime").GetString(), null, System.Globalization.DateTimeStyles.RoundtripKind),
                    Temperature = item.GetProperty("attrValue").GetDouble()
                });
            }

            return Json(temperatureData);
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.UserLogin = HelperController.LoginSessionVerification(HttpContext.Session);
            ViewBag.IsAdmin = HelperController.AdminSessionVerification(HttpContext.Session);
            ViewBag.Theme = HttpContext.Session.GetString("Theme");

            var jsonData = await _temperatureDAO.GetPastData(100);

            var temperatureData = new List<TemperatureViewModel>();
            foreach (var item in jsonData)
            {
                temperatureData.Add(new TemperatureViewModel
                {
                    Timestamp = DateTime.Parse(item.GetProperty("recvTime").GetString(), null, System.Globalization.DateTimeStyles.RoundtripKind),
                    Temperature = item.GetProperty("attrValue").GetDouble()
                });
            }

            return View(temperatureData);
        }
    }
}