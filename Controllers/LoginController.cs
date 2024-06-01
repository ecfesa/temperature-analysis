using Microsoft.AspNetCore.Mvc;
using temperature_analysis.DAO;
using temperature_analysis.Models;
using temperature_analysis.Utils;


namespace temperature_analysis.Controllers
{
    public class LoginController : StandardController<PersonViewModel>
    {

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }

        public IActionResult Login(PersonViewModel model)
        {
            PersonsDAO PersonsDAO = new();
            EmployeesDAO employeesDAO = new();
            ThemeDAO themeDAO = new();

            if (PersonsDAO.LoginExists(model.Username, HashHelper.ComputeSha256Hash(model.PasswordHash)))
            {
                HttpContext.Session.SetString("UserLogin", "true");

                if ((model.Username == AdminLogin.admin_login && model.PasswordHash == AdminLogin.admin_password) || employeesDAO.IsAdmin(model.Username, HashHelper.ComputeSha256Hash(model.PasswordHash)))
                    HttpContext.Session.SetString("IsAdmin", "true");
                else if (employeesDAO.IsEmployee(model.Username, HashHelper.ComputeSha256Hash(model.PasswordHash)))
                    HttpContext.Session.SetString("IsEmployee", "true");

                int id = PersonsDAO.LoginExists(model.Username, HashHelper.ComputeSha256Hash(model.PasswordHash), true);
                var themeId = PersonsDAO.Get(id).ThemeId;
                var theme = themeDAO.Get(themeId);
                  
                HttpContext.Session.SetString("Theme", theme.PrimaryHex);
                HttpContext.Session.SetInt32("ID", id);

                return RedirectToAction("index", "Home");
            }
            else
            {
                return View("Index");
            }
        }

        public IActionResult LogOut()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
    }
}
