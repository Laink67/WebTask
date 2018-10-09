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

        public ActionResult Index(int? id, bool? with)
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

        public ActionResult Find(string SearchText, List<Employee> employess)
        {
            var SpecialEmployee = employess.FindAll(employee => employee.FIO.Contains(SearchText));

            return PartialView("TableEmployee", SpecialEmployee);
        }

        public ActionResult All()
        {
            var employees = DataManager.Instance.Employees.GetAll();

            return PartialView("TableEmployee", employees);
        }

        public ActionResult AllWith(int? id)
        {
            return RedirectToAction("Index",id);
        }

        //public ActionResult AllWith(int? id)
        //{
        //    var departments = DataManager.Instance.Departments.GetAll();
        //    var employees = DataManager.Instance.Employees.GetAllForDepartment((int)id);

        //    ForEmployeesWith(departments, employees, id);

        //    return PartialView("TableEmployee", employees);
        //}

        public ActionResult AllForDepartment(int id)
        {
            var employees = DataManager.Instance.Employees.GetAllForDepartment(id);
            return PartialView("TableEmployee", employees);
        }

        public void ForEmployeesWith(List<Department> departments, List<Employee> employees, int? id)
        {
            foreach (var department in departments)
            {
                if (department.ParentId == id)
                    employees.AddRange(department.Employees);

                if (department.Children.Count != 0)
                    ForEmployeesWith(department.Children, employees, id);
            }
        }
    }
}