using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Threading.Tasks;
using temperature_analysis.DAO;
using temperature_analysis.Models;

namespace temperature_analysis.Controllers
{
    public class DeviceController : Controller
    {
        public async Task<IActionResult> Index()
        {
            ViewBag.Theme = HttpContext.Session.GetString("Theme");
            ViewBag.IsAdmin = HelperController.AdminSessionVerification(HttpContext.Session);

            var devicesDAO = new DeviceDAO();
            var devices = await devicesDAO.GetAll();

            return View("Index", devices);
        }

        [HttpPost]
        public async Task<IActionResult> SaveDevice(DeviceViewModel device)
        {
            if (!ModelState.IsValid)
            {
                return View("DeviceForm", device);
            }

            var devicesDAO = new DeviceDAO();
            try
            {
                var isSuccess = await devicesDAO.Insert(device);
                if (isSuccess)
                    return RedirectToAction("Index");
                else
                    return StatusCode(500, "Failed to insert device.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Failed to insert device.");
            }
        }

        public virtual IActionResult Create()
        {
            try
            {
                ViewBag.Theme = HttpContext.Session.GetString("Theme");
                ViewBag.IsAdmin = HelperController.AdminSessionVerification(HttpContext.Session);
                ViewBag.Operation = "Insert";
                return View("Form", new DeviceViewModel());
            }
            catch (Exception error)
            {
                return View("Error", new ErrorViewModel(error.ToString()));
            }
        }

        public virtual async Task<IActionResult> Details(string deviceId)
        {
            try
            {
                ViewBag.Theme = HttpContext.Session.GetString("Theme");
                ViewBag.IsAdmin = HelperController.AdminSessionVerification(HttpContext.Session);
                ViewBag.Operation = "Update";
                var model = await new DeviceDAO().Get(deviceId);

                if (model == null)
                    return RedirectToAction("Index");

                return View("Form", model);
            }
            catch (Exception error)
            {
                return View("Error", new ErrorViewModel(error.ToString()));
            }
        }

        public async Task<IActionResult> Delete(string deviceId)
        {
            var devicesDAO = new DeviceDAO();
            try
            {
                var isSuccess = await devicesDAO.Delete(deviceId);
                if (isSuccess)
                    return RedirectToAction("Index");
                else
                    return StatusCode(500, "Failed to delete device.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Failed to delete device.");
            }
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!HelperController.AdminSessionVerification(HttpContext.Session))
            {
                context.Result = RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.UserLogin = true;
                base.OnActionExecuting(context);
            }
        }
    }
}
