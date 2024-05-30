using Microsoft.AspNetCore.Mvc;
using System.Reflection.PortableExecutable;
using System;
using temperature_analysis.Models;
using temperature_analysis.DAO;

namespace temperature_analysis.Controllers
{
    public class TemperatureController : StandardController<TemperatureViewModel>
    {
        [Route("temp")]
        public async Task<IActionResult> Get()
        {
            TemperatureDAO dao = new TemperatureDAO();
            var test = await dao.GetPastData(10);


            return Json(new { });
        }

        //integrar com api posteriormente
        public override IActionResult Index()
        {
            string isoDateString = "2023-02-20T15:45:30.123456Z";

            var temperatureData = new List<TemperatureViewModel>
            {
                new TemperatureViewModel { Timestamp = DateTime.Parse(isoDateString, null, System.Globalization.DateTimeStyles.RoundtripKind), Temperature = 22.5 },
                new TemperatureViewModel { Timestamp = DateTime.Parse(isoDateString, null, System.Globalization.DateTimeStyles.RoundtripKind), Temperature = 21.5 },
                new TemperatureViewModel { Timestamp = DateTime.Parse(isoDateString, null, System.Globalization.DateTimeStyles.RoundtripKind), Temperature = 23.0 },
                new TemperatureViewModel { Timestamp = DateTime.Parse(isoDateString, null, System.Globalization.DateTimeStyles.RoundtripKind), Temperature = 22.0 }
            };

            return View(temperatureData);
        }
    }
}
