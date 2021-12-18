using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using AnimeWebApplication.Database;
using AnimeWebApplication.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AnimeWebApplication.Pages
{
    public class AddAnimeItem : PageModel
    {
        public User CurrentUser;
        public Models.Profile CurrentProfile;
        private AnimeItem CurrentAnimeItem;
        
        IWebHostEnvironment _appEnvironment;

        public AddAnimeItem(IWebHostEnvironment appEnvironment)
        {
            _appEnvironment = appEnvironment;
        }
        
        public void OnGet()
        {
            
        }

        public JsonResult OnPostAddItem(
            [FromForm] string name,
            [FromForm] string description,
            [FromForm] string year,
            [FromForm] string ganre,
            [FromForm] string director,
            [FromForm] string countOfSeries
        )
        {
            var animeItem = new AnimeItem()
            {
                ItemId = Guid.NewGuid(),
                Name = name,
                Description = description,
                Year = Convert.ToInt32(year),
                Genre = ganre,
                Director = director,
                SeriesCount = Convert.ToInt32(countOfSeries)
            };

            MyDatabase.Add(animeItem);
            return new JsonResult(animeItem.ItemId);
        }

        public async Task<JsonResult> OnPostUploadPhoto(IFormFile uploadedFile, string itemId)
        {
            var animeItems = MyDatabase.GetAllAnimeItems().Result;
            var animeItem = animeItems.FirstOrDefault(i => i.ItemId == Guid.Parse(itemId));
            var photoPath = "";
            if (uploadedFile != null)
            {
                string path = "/ItemPosters/" + animeItem.Name + uploadedFile.FileName;
                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(fileStream);
                }

                animeItem.PosterPath = path;
                photoPath = path;
            }
            
            MyDatabase.Update(animeItem);
            return new JsonResult(photoPath);
        }

        public async Task<JsonResult> OnGetAnimeItems()
        {
            var items = MyDatabase.GetAllAnimeItems().Result;
            var animeItems = new Dictionary<string, List<string>>();
            var items1 = new List<string>();
            foreach (var item in items)
            {
                items1.Add(item.Name);
            }
            animeItems.Add("items", items1);
            var result = JsonSerializer.Serialize<Dictionary<string, List<string>>>(animeItems);
            return new JsonResult(result);
        }
         
        
        private User FindUserByToken()
        {
            var stream = Request.Cookies["token"];
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(stream);
            var tokenS = jsonToken as JwtSecurityToken;
            var id = tokenS.Claims.First(claim => claim.Type == "nameid").Value;
            var users = MyDatabase.GetAllUsers().Result;
            var user = users.FirstOrDefault(u => u.Id.ToString() == id);
            return user;
        }
        
        private void InitCurrentUser()
        {
            if (Request.Cookies["token"] == null || Request.Cookies["token"] == "")
            {
                CurrentUser = null;
            }
            else
            {
                CurrentUser = FindUserByToken();
            }
        }
        public void InitProfile()
        {
            var profiles = MyDatabase.GetAllProfiles().Result;
            CurrentProfile = profiles.FirstOrDefault(p => p.Id == CurrentUser.Id);
        }
    }
}