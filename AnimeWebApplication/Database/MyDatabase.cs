using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AnimeWebApplication.Models;
using Npgsql;
using Profile = AnimeWebApplication.Pages.Profile;

namespace AnimeWebApplication.Database
{
    public class MyDatabase
    {
        private static string ConnectionString =
            "User ID=postgres; Server=localhost; port=5432; Database=anime_web_app_db; Password=admin; Pooling=true;";

        private static NpgsqlConnection Connection = new NpgsqlConnection(ConnectionString);

        private static string UserProperties = "id, username, email, password";
        private static string ProfileProperties = "id, birthday, city, description, sex";
        
        private static string UserTable = "users";
        private static string ProfileTable = "profiles";

        public static async Task Add(User user)
        {
            await Connection.OpenAsync();
            
            var userValues = GetValues(user);
            var comm = $"INSERT INTO \"{UserTable}\" ({UserProperties}) VALUES ({userValues});";
            var cmd = new NpgsqlCommand(comm, Connection);
            await cmd.ExecuteNonQueryAsync();

            await Connection.CloseAsync();
        }
        
        public static async Task Add(Models.Profile profile)
        {
            await Connection.OpenAsync();
            
            var profileValues = GetValues(profile);
            var comm = $"INSERT INTO \"{ProfileTable}\" ({ProfileProperties}) VALUES ({profileValues});";
            var cmd = new NpgsqlCommand(comm, Connection);
            await cmd.ExecuteNonQueryAsync();
            
            await Connection.CloseAsync();
        }

        public static async Task AddRange(IEnumerable<User> users)
        {
            await Connection.OpenAsync();

            foreach (var user in users)
            {
                var userValues = GetValues(user);
                var comm = $"INSERT INTO \"{UserTable}\" ({UserProperties}) VALUES ({userValues});";
                var cmd = new NpgsqlCommand(comm, Connection);
                await cmd.ExecuteNonQueryAsync();
            }

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
        
        public static async Task<List<Models.Profile>> GetAllProfiles()
        {
            await Connection.OpenAsync();
            
            var profiles = new List<Models.Profile>();
            
            var cmd = new NpgsqlCommand($"SELECT * FROM \"{ProfileTable}\"", Connection);
            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                profiles.Add(new Models.Profile()
                {
                    Id = reader.GetGuid(0),
                    Birthday = reader.GetString(1),
                    City = reader.GetString(2),
                    Description = reader.GetString(3),
                });
            }
            await Connection.CloseAsync();
            
            return profiles;
        }

        public static async void Update(Models.Profile profile)
        {
            await Connection.OpenAsync();
            
            var profileValues = GetValues(profile);
            var comm = $"UPDATE \"{ProfileTable}\" SET ({ProfileProperties}) = ({profileValues}) WHERE id='{profile.Id}'";
            var cmd = new NpgsqlCommand(comm, Connection);
            await cmd.ExecuteNonQueryAsync();

            await Connection.CloseAsync();
        }

        private static string GetValues(User user) =>
            $"'{user.Id}', '{user.Username}', '{user.Email}', '{user.Password}'";
        private static string GetValues(Models.Profile profile) =>
            $"'{profile.Id}', '{profile.Birthday}', '{profile.City}', '{profile.Description}', '{profile.Sex}'";
    }
}