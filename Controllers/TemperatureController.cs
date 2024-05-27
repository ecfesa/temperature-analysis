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
            

            return Json(new {});
        }
    }
}
