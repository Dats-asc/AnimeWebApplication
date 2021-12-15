using System;
using System.Collections.Generic;
using System.Data;
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
        private static string ProfileProperties = "id, birthday, city, description, sex, photo_path";
        private static string UserPhotoProperties = "id, name, path";
        private static string AnimePropertires = "item_id, name, description, year, genre, director, series_count, poster_path";
        
        private static string UserTable = "users";
        private static string ProfileTable = "profiles";
        private static string UserPhotoTable = "user_photos";
        private static string AnimeTable = "anime_item";

        public static async Task Add(User user)
        {
            await Connection.OpenAsync();
            
            var userValues = GetValues(user);
            var comm = $"INSERT INTO \"{UserTable}\" ({UserProperties}) VALUES ({userValues});";
            var cmd = new NpgsqlCommand(comm, Connection);
            await cmd.ExecuteNonQueryAsync();

            await Connection.CloseAsync();
        }
        
        public static async Task Add(AnimeItem anime)
        {
            await Connection.OpenAsync();
            
            var userValues = GetValues(anime);
            var comm = $"INSERT INTO \"{AnimeTable}\" ({AnimePropertires}) VALUES ({userValues});";
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
        
        public static async Task Add(UserPhoto photo)
        {
            await Connection.OpenAsync();
            
            var profileValues = GetValues(photo);
            var comm = $"INSERT INTO \"{UserPhotoTable}\" ({UserPhotoTable}) VALUES ({profileValues});";
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
        
        public static async Task<List<AnimeItem>> GetAllAnimeItems()
        {
            await Connection.OpenAsync();
            
            var animes = new List<AnimeItem>();
            
            var cmd = new NpgsqlCommand($"SELECT * FROM \"{AnimeTable}\"", Connection);
            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                animes.Add(new AnimeItem()
                {
                    ItemId = reader.GetGuid(0),
                    Name = reader.GetString(1),
                    Description = reader.GetString(2),
                    Year = reader.GetInt32(3),
                    Genre = reader.GetString(4),
                    Director = reader.GetString(5),
                    SeriesCount = reader.GetInt32(6),
                    PosterPath = reader.GetString(7)
                });
            }
            await Connection.CloseAsync();
            
            return animes;
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
                    PhotoPath = reader.GetString(5)
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
        
        public static async void Update(AnimeItem animeItem)
        {
            await Connection.OpenAsync();
            
            var animeProperties = GetValues(animeItem);
            var comm = $"UPDATE \"{AnimeTable}\" SET ({AnimePropertires}) = ({animeProperties}) WHERE item_id='{animeItem.ItemId}'";
            var cmd = new NpgsqlCommand(comm, Connection);
            await cmd.ExecuteNonQueryAsync();

            await Connection.CloseAsync();
        }

        private static string GetValues(User user) =>
            $"'{user.Id}', '{user.Username}', '{user.Email}', '{user.Password}'";
        private static string GetValues(Models.Profile profile) =>
            $"'{profile.Id}', '{profile.Birthday}', '{profile.City}', '{profile.Description}', '{profile.Sex}', '{profile.PhotoPath}'";
        private static string GetValues(UserPhoto photo) =>
            $"'{photo.Id}', '{photo.Name}', '{photo.Path}'";
        private static string GetValues(AnimeItem anime) =>
            $"'{anime.ItemId}', '{anime.Name}', '{anime.Description}', '{anime.Year}', '{anime.Genre}', '{anime.Director}', '{anime.SeriesCount}', '{anime.PosterPath}'";
    }
}