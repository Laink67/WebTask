using Dal.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace Dal.DBObjects
{
    /// <summary>
    /// Экземпляр отдела из БД
    /// </summary>
    public class Department : BasedObject
    {
        public string Title { get; set; }

        public string Comment { get; set; }

        public int? ParentId { get; set; }

        public List<Department> Children { get; set; } = new List<Department>();

        public List<Employee> Employees { get; set; }

        public List<Employee> EmployeesWith { get; set; }
        //    Children.Select(x => x.EmployeesWith).Cast<Employee>().Union(Employees).ToList();

        public int PensionersCount  => Employees.Where(x => x.Retired).Count(); 

        public Department() { }

        public Department(int id, string title, string comment, int? parentid)
        {
            Id = id;
            Title = title;
            Comment = comment;
            ParentId = parentid;
            Employees = DataManager.Instance.Employees.GetAllForDepartment(id).ToList();
        }

    }
}