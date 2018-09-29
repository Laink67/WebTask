using CacheLayer;

namespace Dal.Repositories
{
    /// <summary>
    /// Реализация паттерна Singleton для репозиториев  и паттерна Unit of Work  для инкапсулирования логики работы с источником данных.
    /// </summary>
    public class DataManager
    {
        private static DataManager _instance;


        private DataManager()
        {
            Departments = new DepartmentRepository();
            Employees = new EmployeeRepository();
        }

        public static DataManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new DataManager();
                return _instance;

            }
        }


        public DepartmentRepository Departments { get; }

        public EmployeeRepository Employees { get; }
    }
}
