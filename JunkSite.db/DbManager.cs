using System;
using System.Collections.Generic;
using System.Data.SqlClient;
namespace JunkSite.db
{
    public class DbManager
    {
        private string _connectionString;
        public DbManager(string constr)
        {
            _connectionString = constr;
        }
        public int AddJunk(Junk j)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = connection.CreateCommand())
            {
                command.CommandText = @"INSERT INTO Junk(Name, Date, Details, Phone)
                                        VALUES (@name, @date, @details, @phone) 
                                        SELECT SCOPE_IDENTITY()";
                command.Parameters.AddWithValue("@name", j.Name);
                command.Parameters.AddWithValue("@date", DateTime.Now);
                command.Parameters.AddWithValue("@details", j.Details);
                command.Parameters.AddWithValue("@phone", j.Phone);
                connection.Open();
                int id= (int)(decimal)command.ExecuteScalar();
                return id;
            }
        }
        public List<Junk> GetAllJunk()
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = connection.CreateCommand())
            {
                command.CommandText = @"SELECT * FROM Junk";
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
               List<Junk> list= new List<Junk>();
                while (reader.Read())
                {
                    list.Add(new Junk
                    {
                        Date = (DateTime)reader["date"],
                        Id = (int)reader["id"],
                        Name = (string)reader["name"],
                        Details = (string)reader["details"],
                        Phone = (string)reader["phone"]
                    });
                }
                return list;
            }
         
        }
        public void Delete(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = connection.CreateCommand())
            {
                command.CommandText = @"DELETE FROM Junk WHERE Id = @id";
                command.Parameters.AddWithValue("@id", id);
                connection.Open();
                command.ExecuteNonQuery();
            }

        }
        
    }
}
