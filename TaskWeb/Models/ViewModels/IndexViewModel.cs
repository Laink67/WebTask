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

        public IndexViewModel(List<Employee> employees, int? id)
        {
            SelectedId = id;
            Employees = employees;
        }

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

        //public IndexViewModel(List<Department> departments, int? id)
        //{
        //    SelectedId = id;
        //    Check(departments);
        //    Departments = departments;
        //    ForEmployeesWith(Departments,Employees,id);
        //}

        //public void ForEmployeesWith(List<Department> departments,List<Employee> employees,int? id)
        //{
        //    foreach (var department in departments)
        //    {
        //        if(department.ParentId == id || department.Id == id)
        //        employees.AddRange(department.Employees);

        //        if (department.Children.Count != 0)
        //            ForEmployeesWith(department.Children,employees,id);
        //    }
        //}

        //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        //public void CheckPensioners(IEnumerableIe<Department> departments, IEnumerableIe<Employee> employees)
        //{
        //    foreach (var department in departments)
        //    {
        //        department.PensionersCount = 0;
        //        department.EmployeesCount = 0;

        //        foreach (var employee in employees)
        //        {
        //            if (employee.DepartmentId == department.Id)
        //            {
        //                department.EmployeesCount++;

        //                var span = DateTime.Now.Date - employee.Date;

        //                if (((int)employee.Gender == 1 && span.Days >= 65 * 365) ||
        //                    (employee.Gender == 0 && span.Days >= 60 * 365))
        //                {
        //                    department.PensionersCount++;

        //                    if (department.PensionersCount > 2)
        //                        SelectedPensioners.Add(department.Id);
        //                }
        //            }
        //        }



        //        if (department.Children.Count != 0)
        //            CheckPensioners(department.Children, employees);
        //    }
        //}

        //public void Check(IEnumerableIe<Department> departments, IEnumerableIe<Employee> employees)
        //{
        //    departments.ForEach(department => department.Employees = employees.Where(employee => employee.DepartmentId == department.Id)); // Не проходит по детям
        //}

    }
}