using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AnimeWebApplication.Models;
using Npgsql;

namespace AnimeWebApplication.Database
{
    public class MyDatabase
    {
        private static string ConnectionString =
            "User ID=postgres; Server=localhost; port=5432; Database=anime_web_app_db; Password=admin; Pooling=true;";

        private static NpgsqlConnection Connection = new NpgsqlConnection(ConnectionString);

        private static string UserProperties = "id, username, email, password";

        private static string UserTable = "users";

        public static async Task Add(User user)
        {
            await Connection.OpenAsync();
            
            var userValues = GetValues(user);
            var comm = $"INSERT INTO \"{UserTable}\" ({UserProperties}) VALUES ({userValues});";
            var cmd = new NpgsqlCommand(comm, Connection);
            await cmd.ExecuteNonQueryAsync();

            await Connection.CloseAsync();
        }

        public static async Task AddRange(IEnumerable<User> users)
        {
            await Connection.OpenAsync();
            
            /*var comm = $"INSERT INTO \"{UserTable}\" ({UserProperties}) VALUES ;";
            foreach (var user in users)
            { 
                var userValues = GetValues(user);
                if (user.Id != users.Last().Id)
                {
                    comm += $"({userValues}), ";
                }
                else
                {
                    comm += $"({userValues});";
                }
            }*/

            foreach (var user in users)
            {
                var userValues = GetValues(user);
                var comm = $"INSERT INTO \"{UserTable}\" ({UserProperties}) VALUES ({userValues});";
                var cmd = new NpgsqlCommand(comm, Connection);
                await cmd.ExecuteNonQueryAsync();
            }
            
            //var cmd = new NpgsqlCommand(comm, Connection);
            //await cmd.ExecuteNonQueryAsync();

            await Connection.CloseAsync();
        }

        public static async Task<List<User>> GetAllUsers()
        {
            await Connection.OpenAsync();
            
            var users = new List<User>();
            
            var cmd = new NpgsqlCommand($"SELECT * FROM \"{UserTable}\"", Connection);
            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                users.Add(new User()
                {
                    Id = reader.GetGuid(0),
                    Username = reader.GetString(1),
                    Email = reader.GetString(2),
                    Password = reader.GetString(3),
                });
            }
            await Connection.CloseAsync();
            
            return users;
        }

        private static string GetValues(User user) =>
            $"'{user.Id}', '{user.Username}', '{user.Email}', '{user.Password}'";
    }
}