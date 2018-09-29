using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Dal.Common;
using Dal.DBObjects;

namespace Dal.Repositories
{
    public class EmployeeRepository : BaseRepository<Employee>
    {
        public override Employee Create(SqlDataReader reader)
        {
            var employee = new Employee((int)reader["Id"],
                reader["FIO"].ToString(),
                (DateTime)reader["Date"],
                reader["Phone"].ToString(),
                (Gender)reader["Gender"],
                (int)reader["DepartmentId"],
                reader["Comment"].ToString());
            return employee;
        }

        public List<Employee> GetAllForDepartment(int departmentid)
        {
            using (var command = new SqlCommand($"select * from {nameof(Employee)} where DepartmentId = {departmentid}", connection))
            {
                return GetRecords(command);
            }
        }

        public void Add(Employee model)
        {
            var command = Insert(
                $"@{nameof(Employee.FIO)}," +
                $"@{nameof(Employee.Date)}," +
                $"@{nameof(Employee.Gender)}," +
                $"@{nameof(Employee.Phone)}," +
                $"@{nameof(Employee.DepartmentId)}," +
                $"@{nameof(Employee.Comment)}");

            if (model.Comment == null)
                command.Parameters.AddWithValue($"@{nameof(Employee.Comment)}", DBNull.Value);
            else
                command.Parameters.AddWithValue($"@{nameof(Employee.Comment)}", model.Comment);

            command.Parameters.AddWithValue($"@{nameof(Employee.FIO)}", model.FIO);
            command.Parameters.AddWithValue($"@{nameof(Employee.Date)}", model.Date);
            command.Parameters.AddWithValue($"@{nameof(Employee.Gender)}", model.Gender);
            command.Parameters.AddWithValue($"@{nameof(Employee.Phone)}", model.Phone);
            command.Parameters.AddWithValue($"@{nameof(Employee.DepartmentId)}", model.DepartmentId);

            Execute(command);
        }

        public void Edit(Employee model)
        {
            var command = Update(
                $"{nameof(Employee.FIO)}=@{nameof(Employee.FIO)}," +
                $"{nameof(Employee.Date)}=@{nameof(Employee.Date)}," +
                $"{nameof(Employee.Gender)}=@{nameof(Employee.Gender)}," +
                $"{nameof(Employee.Phone)}=@{nameof(Employee.Phone)}," +
                $"{nameof(Employee.DepartmentId)}=@{nameof(Employee.DepartmentId)}," +
                $"{nameof(Employee.Comment)}=@{nameof(Employee.Comment)}");

            command.Parameters.AddWithValue($"@{nameof(BasedObject.Id)}", model.Id);
            command.Parameters.AddWithValue($"@{nameof(Employee.FIO)}", model.FIO);
            command.Parameters.AddWithValue($"@{nameof(Employee.Date)}", model.Date);
            command.Parameters.AddWithValue($"@{nameof(Employee.Gender)}", model.Gender);
            command.Parameters.AddWithValue($"@{nameof(Employee.Phone)}", model.Phone);
            command.Parameters.AddWithValue($"@{nameof(Employee.DepartmentId)}", model.DepartmentId);


            if (model.Comment == null)
                command.Parameters.AddWithValue($"@{nameof(Employee.Comment)}", DBNull.Value);
            else
                command.Parameters.AddWithValue($"@{nameof(Employee.Comment)}", model.Comment);

            Execute(command);
        }
    }
}
