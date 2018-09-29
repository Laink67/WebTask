using System.Collections.Generic;

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

        public IEnumerable<Employee> Employees { get;set }

        public int EmployeesCount { get; set; } //=>

        public int EmployeesWithCount { get; set; } //=>

        public int PensionersCount { get; set; } //=>

        // public ienumerable<employee> employees 

        public Department() { }

        public Department(int id, string title, string comment, int? parentid)
        {
            Id = id;
            Title = title;
            Comment = comment;
            ParentId = parentid;

        }


    }
}