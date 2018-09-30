using System.Web.Mvc;
using TaskWeb.Models.ViewModels;
using Dal.Repositories;
using Dal.DBObjects;
using System.Collections.Generic;

namespace TaskWeb.Controllers
{
    /// <summary>
    /// Вывод списка отделов и соответсвующих сотрудников
    /// </summary>
    public class HomeController : Controller
    {
        public DataManager dataManager;

        public HomeController()
        {
            dataManager = DataManager.Instance;
        }

        public ActionResult Index(int? id,bool? with)
        {
            IndexViewModel model;
            var depts = dataManager.Departments.GetAllForTree();

            for (int i = 0; i < depts.Count; i++)
            {
                for (int j = 0; j < depts.Count; j++)
                {
                    if (depts[i].Id == depts[j].ParentId)
                        depts[i].Children.Add(depts[j]);
                }

                if (depts[i].ParentId.HasValue)
                {
                    depts.RemoveAt(i);
                    i--;
                }
            }

            if (!with.HasValue)
            {
                var allEmployees = dataManager.Employees.GetAll();
                var employees = id.HasValue ? dataManager.Employees.GetAllForDepartment((int)id) : allEmployees;
                model = new IndexViewModel(depts, employees, id);
            }
            else
            {
                model = new IndexViewModel(depts, id);
            }

            return View(model);
        }

        public ActionResult All()
        {
            return RedirectToAction("Index", "Home");
        }

        public ActionResult AllWith(int? id)
        {
            return RedirectToAction("Index", "Home",new { id,with = true});
        }

    }
}