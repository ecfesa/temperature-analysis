using Microsoft.AspNetCore.Mvc;
using temperature_analysis.DAO;
using temperature_analysis.Models;

namespace temperature_analysis.Controllers
{
    public class StandardController<T> : Controller where T : StandardViewModel
    {
        // Standard Controller for CRUD operations
        protected bool NeedsAuthentication { get; set; } = true;
        protected StandardDAO<T>? DAO { get; set; }

        // Default view names
        protected string IndexViewName { get; set; } = "Index";
        protected string FormViewName { get; set; } = "Form";

        public virtual IActionResult Index()
        {
            try
            {
                return View();
            }
            catch (Exception error)
            {
                return View("Error", new ErrorViewModel(error.ToString()));
            }
        }

        public virtual IActionResult Create()
        {
            try
            {
                ViewBag.Operation = "Insert";
                return View(FormViewName, Activator.CreateInstance(typeof(T)) as T);
            }
            catch (Exception error)
            {
                return View("Error", new ErrorViewModel(error.ToString()));
            }
        }

        // Saves the item in the database
        public virtual IActionResult Save(T model, string operation)
        {
            if (DAO == null)
                throw new ArgumentNullException("DAO", "DAO was not defined the controller.");


            try
            {
                if (operation == "Insert")
                    DAO.Insert(model);
                else
                    DAO.Update(model);

                return RedirectToAction(IndexViewName);
            }
            catch (Exception error)
            {
                return View("Error", new ErrorViewModel(error.ToString()));
            }
        }

        public virtual IActionResult Edit(int id)
        {
            if (DAO == null)
                throw new ArgumentNullException("DAO", "DAO was not defined the controller.");

            try
            {
                ViewBag.Operation = "Update";
                var model = DAO.Get(id);

                if (model == null)
                    return RedirectToAction(IndexViewName);

                return View(FormViewName, model);
            }
            catch (Exception error)
            {
                return View("Error", new ErrorViewModel(error.ToString()));
            }
        }

        // Deletes the item in the database
        public virtual IActionResult Delete(int id, string id_route)
        {
            if (DAO == null)
                throw new ArgumentNullException("DAO", "DAO was not defined the controller.");

            try
            {
                DAO.Delete(id, id_route);
                return RedirectToAction(IndexViewName);
            }
            catch (Exception error)
            {
                return View("Error", new ErrorViewModel(error.ToString()));
            }
        }



    }
}
