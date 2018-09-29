using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Dal.DBObjects;

namespace Dal.Repositories
{
    public class DepartmentRepository : BaseRepository<Department>
    {
        public override Department Create(SqlDataReader reader)
        {
            var department = new Department((int)(reader["Id"]),
                                          reader["Title"].ToString(),
                                          reader["Comment"].ToString(),
                                          reader["ParentId"] == DBNull.Value ? null : (int?)reader["ParentId"]);
            return department;
        }

        public List<Department> GetAllForTree()
        {
            if (Cache.IsInMemory($"All{typeof(Department).Name}ForTree"))
                return Cache.FetchData<Department>($"All{typeof(Department).Name}ForTree");
            using (var command = new SqlCommand($"select * from {typeof(Department).Name}", connection))
            {
                var records = GetRecords(command);
                Cache.Add($"All{typeof(Department).Name}ForTree", records, 60);
                return records;
            }
        }

        public void Add(Department model)
        {
            Cache.Remove($"All{typeof(Department).Name}ForTree");

            var command = Insert(
             $"@{nameof(Department.Title)}," +
             $"@{nameof(Department.Comment)}," +
             $"@{nameof(Department.ParentId)}");

            if (model.Comment == null)
                command.Parameters.AddWithValue($"@{nameof(Department.Comment)}", DBNull.Value);
            else
                command.Parameters.AddWithValue($"@{nameof(Department.Comment)}", model.Comment);

            if (!model.ParentId.HasValue)
                command.Parameters.AddWithValue($"@{nameof(Department.ParentId)}", DBNull.Value);
            else
                command.Parameters.AddWithValue($"{nameof(Department.ParentId)}", model.ParentId);

            command.Parameters.AddWithValue($"@{nameof(Department.Title)}", model.Title);

            Execute(command);
        }

        public void Edit(Department model)
        {
            Cache.Remove($"All{typeof(Department).Name}ForTree");

            var command = Update(
                $"{nameof(Department.Title)}=@{nameof(Department.Title)}," +
                $"{nameof(Department.Comment)}=@{nameof(Department.Comment)}");

            command.Parameters.AddWithValue($"@{nameof(BasedObject.Id)}", model.Id);
            command.Parameters.AddWithValue($"@{nameof(Department.Title)}", model.Title);

            if (model.Comment == null)
                command.Parameters.AddWithValue($"@{nameof(Department.Comment)}", DBNull.Value);
            else
                command.Parameters.AddWithValue($"@{nameof(Department.Comment)}", model.Comment);

            Execute(command);
        }
    }
}
