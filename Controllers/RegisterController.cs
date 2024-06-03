using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using temperature_analysis.DAO;
using temperature_analysis.Models;

namespace temperature_analysis.Controllers
{
    public class RegisterController : StandardController<PersonViewModel>
    {
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }

        public override IActionResult Index()
        {
            ViewBag.Themes = new ThemeDAO().GetAll().Select(t => new SelectListItem(t.Description, t.Id.ToString()));
            return View(new PersonViewModel());
        }

        public override IActionResult Save(PersonViewModel model, string operation)
        {
            PersonsDAO DAO = new PersonsDAO();

            model.PasswordHash = HashHelper.ComputeSha256Hash(model.PasswordHash);

            ValidateRegistry(model);

            if (model.FormImg != null)
                using (var ms = new MemoryStream())
                {
                    model.FormImg.CopyTo(ms);
                    model.ByteArrImg = ms.ToArray();
                }

            if (ModelState.IsValid == true)
            {
                DAO.Insert(model);
                return RedirectToAction("index", "Home");
            }

            return View("Index", model);
        }

        public void ValidateRegistry(PersonViewModel model)
        {
            ModelState.Clear();

            if (string.IsNullOrEmpty(model.PasswordHash) || model.PasswordHash.Length < 8)
                ModelState.AddModelError("PasswordHash", "The Password must be longer than 8 caracthers!!!");
        }
    }
}