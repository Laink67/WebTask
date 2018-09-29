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
        public List<Employee> Employees { get; set; }
        public int? SelectedId { get; set; }
        public List<int> SelectedPensioners { get; set; } = new List<int>();

        public IndexViewModel(List<Department> departments, List<Employee> employees, int? id, List<Employee> allEmployees)
        {
            SelectedId = id;
            Employees = employees;
            CheckPensioners(departments, allEmployees);
            Departments = departments;
        }

        //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        public void CheckPensioners(List<Department> departments, List<Employee> employees)
        {
            foreach (var department in departments)
            {
                department.PensionersCount = 0;
                department.EmployeesCount = 0;

                foreach (var employee in employees)
                {
                    if (employee.DepartmentId == department.Id)
                    {
                        department.EmployeesCount++;

                        var span = DateTime.Now.Date - employee.Date;

                        if (((int)employee.Gender == 1 && span.Days >= 65 * 365) ||
                            (employee.Gender == 0 && span.Days >= 60 * 365))
                        {
                            department.PensionersCount++;

                            if (department.PensionersCount > 2)
                                SelectedPensioners.Add(department.Id);
                        }
                    }
                }

                

                if (department.Children.Count != 0)
                    CheckPensioners(department.Children, employees);
            }
        }
    }
}