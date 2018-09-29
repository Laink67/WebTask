using System.Configuration;
using System.Data.SqlClient;
using System.Collections.Generic;
using Dal.DBObjects;
using CacheLayer;

namespace Dal
{
    /// <summary>
    /// Класс для получение и изменения данных из БД
    /// </summary>  
    public abstract class BaseRepository<T> where T : BasedObject
    {
        private DefaultCacheProvider<T> _cache;
        public string connectionString = ConfigurationManager.ConnectionStrings["Connection"].ConnectionString;
        public SqlConnection connection = new SqlConnection();

        public DefaultCacheProvider<T> Cache
        {
            get
            {
                if (_cache == null)
                    _cache = new DefaultCacheProvider<T>();
                return _cache;
            }

        }

        public abstract T Create(SqlDataReader reader);

        public List<T> GetRecords(SqlCommand command)
        {
            var list = new List<T>();
            command.Connection = connection;
            connection.ConnectionString = connectionString;

            using (connection)
            {
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(Create(reader));
                    }
                }
            }
            return list;
        }

        public List<T> GetAll()
        {
            if (Cache.IsInMemory($"All{typeof(T).Name}"))
                return Cache.FetchData<T>($"All{typeof(T).Name}");
            else
            {
                using (var command = new SqlCommand($"select * from {typeof(T).Name}", connection))
                {
                    var records = GetRecords(command);
                    Cache.Add($"All{typeof(T).Name}", records, 60);
                    return records;
                }
            }
        }

        public T Get(int id)
        {
            if (Cache.IsInMemory($"{typeof(T).Name + id.ToString()}"))
                return Cache.Get($"{typeof(T).Name + id.ToString()}");
            else
            {
                using (var command = new SqlCommand($"select * from {typeof(T).Name} where {nameof(BasedObject.Id)} = {id}", connection))
                {
                    var record = GetRecord(command);
                    Cache.Add(typeof(T).Name, record, 60);
                    return record;
                }
            }
        }


        public T GetRecord(SqlCommand command)
        {
            T record = null;
            connection.ConnectionString = connectionString;
            command.Connection = connection;
            using (connection)
            {
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        record = Create(reader);
                        break;
                    }
                }
            }
            return record;
        }

        public void Execute(SqlCommand command)
        {
            connection.ConnectionString = connectionString;
            using (connection)
            {
                connection.Open();
                using (command)
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public SqlCommand Insert(string Parametrs)
        {
            Cache.Remove($"All{typeof(T).Name}");

            return new SqlCommand($"Insert into {typeof(T).Name} values({Parametrs})", connection);
        }

        public SqlCommand Update(string Parametrs)
        {
            Cache.Remove($"All{typeof(T).Name}");

            return new SqlCommand($"Update {typeof(T).Name} set {Parametrs} where {nameof(BasedObject.Id)}=@{nameof(BasedObject.Id)}", connection);
        }

        public void Remove(int id)
        {
            Cache.Remove($"All{typeof(T).Name}");

            Cache.Remove($"All{typeof(Department).Name}ForTree");                     //КОСТЫЛЬ    КОСТЫЛЬ         КОСТЫЛЬ       КОСТЫЛЬ

            connection.ConnectionString = connectionString;

            using (connection)
            {
                var command = new SqlCommand($"Delete from {typeof(T).Name} where {nameof(BasedObject.Id)}=@{nameof(BasedObject.Id)}", connection);
                command.Parameters.AddWithValue($"@{nameof(BasedObject.Id)}", id);

                connection.Open();
                using (command)
                {
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
