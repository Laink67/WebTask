using Dal.DBObjects;
using Dal.Repositories;
using System.Collections.Generic;
using System.Web.Mvc;
using TaskWeb.Models.ViewModels;

namespace TaskWeb.Controllers
{
    /// <summary>
    /// Добавление, редактирование, удаление отдела
    /// </summary>
    public class DepartmentController : Controller
    {
        public ActionResult Edit(int? id, int? parentid)
        {
            DepartmentViewModel model;

            if (!id.HasValue)
            {
                model = new DepartmentViewModel(parentid);
            }
            else
            {
                var department = DataManager.Instance.Departments.Get((int)id);
                model = new DepartmentViewModel(department);
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(DepartmentViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Id == 0)
                    DataManager.Instance.Departments.Add(model.GetDepartment());
                else
                    DataManager.Instance.Departments.Edit(model.GetDepartment());

                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View(model);
            }
        }

        public ActionResult Delete(int id)
        {
            var departments = DataManager.Instance.Departments.GetAll();
            var hasChildren = true;

            Check(ref hasChildren, departments, id);

            if (DataManager.Instance.Employees.GetAllForDepartment(id).Count == 0 && hasChildren) /*DataManager.Instance.Departments.Get(id).Childrens.Count == 0*/
                DataManager.Instance.Departments.Remove(id);

            return RedirectToAction("Index", "Home");
        }

        public void Check(ref bool hasChildren, List<Department> departments, int id)
        {
            foreach (var department in departments)
            {
                if (department.ParentId == id)
                {
                    hasChildren = false;
                    break;
                }
                Check(ref hasChildren, department.Children, id);
            }
        }
    }
}