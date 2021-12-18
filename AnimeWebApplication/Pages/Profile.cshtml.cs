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
    public class Profile : PageModel
    {
        public User CurrentUser;
        public Models.Profile CurrentProfile;
        IWebHostEnvironment _appEnvironment;

        public Profile(IWebHostEnvironment appEnvironment)
        {
            _appEnvironment = appEnvironment;
        }
        
        public IActionResult OnGet()
        {
            if (Request.Cookies["token"] != null)
            {
                InitCurrentUser();
                InitProfile();
                return Page();
            }
            else
            {
                return Redirect("/login");
            }
        }

        public JsonResult OnPostProfileChanged([FromForm]string birthday, [FromForm]string sex, [FromForm]string city, [FromForm]string description)
        {
            InitCurrentUser();
            InitProfile();
            CurrentProfile.Birthday = birthday;
            CurrentProfile.City = city;
            CurrentProfile.Description = description;
            CurrentProfile.Sex = sex;
            
            MyDatabase.Update(CurrentProfile);
            return new JsonResult("ok");
        }
        
        public async Task<JsonResult> OnPostUploadPhoto(IFormFile uploadedFile)
        {
            InitCurrentUser();
            InitProfile();
            var photoPath = "";
            if (uploadedFile != null)
            {
                // путь к папке Files
                string path = "/UserPhotos/"+ CurrentUser.Username + uploadedFile.FileName;
                // сохраняем файл в папку Files в каталоге wwwroot
                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(fileStream);
                }
                photoPath = path;
                CurrentProfile.PhotoPath = path;
                MyDatabase.Update(CurrentProfile);
            }
            return new JsonResult(photoPath);
        }

        public JsonResult OnGetUserProfile()
        {
            InitCurrentUser();
            var profiles = MyDatabase.GetAllProfiles().Result;
            var profile = profiles.FirstOrDefault(p => p.Id == CurrentUser.Id);

            var result = JsonSerializer.Serialize(profile);
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