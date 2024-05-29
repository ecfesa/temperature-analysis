using Microsoft.AspNetCore.Mvc;
using temperature_analysis.DAO;
using temperature_analysis.Models;

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
    }
}
