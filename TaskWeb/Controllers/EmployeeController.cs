using Dal.DBObjects;
using Dal.Repositories;
using System.Web.Mvc;
using TaskWeb.Models.ViewModels;

namespace TaskWeb.Controllers
{
    /// <summary>
    /// Добавление, редактирование, удаление сотрудника
    /// </summary>
    public class EmployeeController : Controller
    {
        public ActionResult Edit(int? id, int? departmentid)
        {
            EmployeeViewModel model;
            Employee employee;

            if (id.HasValue)
            {
                employee = DataManager.Instance.Employees.Get((int)id);
                var departments = DataManager.Instance.Departments.GetAll();
                model = new EmployeeViewModel(employee, departments);
            }
            else
            {
                employee = new Employee();
                if (departmentid.HasValue)
                {
                    var department = DataManager.Instance.Departments.Get((int)departmentid);
                    model = new EmployeeViewModel(employee, department.Title);
                }
                else
                {
                    var departments = DataManager.Instance.Departments.GetAll();
                    model = new EmployeeViewModel(employee, departments);
                }
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(EmployeeViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Id == 0)
                    DataManager.Instance.Employees.Add(model.GetEmployee());
                else
                    DataManager.Instance.Employees.Edit(model.GetEmployee());

                return RedirectToAction($"Index/{model.DepartmentId}", "Home");
            }
            else
            {
                return View(model);
            }
        }

        public ActionResult Delete(int id)
        {
            var departmentid = DataManager.Instance.Employees.Get(id).DepartmentId;
            DataManager.Instance.Employees.Remove(id);
            return RedirectToAction($"Index/{departmentid}", "Home");
        }
    }
}