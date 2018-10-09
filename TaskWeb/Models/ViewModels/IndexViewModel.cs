using System;
using System.Collections.Generic;
using System.Linq;
using Dal.DBObjects;

namespace TaskWeb.Models.ViewModels
{
    /// <summary>
    /// Модель для передачи на главную страницу
    /// </summary>
    public class IndexViewModel
    {
        public List<Department> Departments { get; set; } = new List<Department>();
        public List<Employee> Employees { get; set; } = new List<Employee>();
        public int? SelectedId { get; set; }
        public List<int> SelectedPensioners { get; set; } = new List<int>();

        public IndexViewModel(List<Department> departments, List<Employee> employees, int? id)
        {
            SelectedId = id;
            Employees = employees;
            Check(departments);
            Departments = departments;
        }

        public void Check(List<Department> departments)
        {
            foreach (var department in departments)
            {
                if (department.PensionersCount > 2)
                    SelectedPensioners.Add(department.Id);

                if (department.Children.Count != 0)
                    Check(department.Children);
            }
        }
    }
}