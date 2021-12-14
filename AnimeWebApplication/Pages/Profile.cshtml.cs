using System;
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

        public Profile()
        {
            IWebHostEnvironment appEnvironment;
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
            var user = FindUserByToken();
            var newProfile = new Models.Profile()
            {
                Id = user.Id,
                Birthday = birthday,
                City = city,
                Description = description,
                Sex = sex
            };
            MyDatabase.Update(newProfile);
            return new JsonResult("ok");
        }

        public async Task<PageResult> OnPost([Bind("ImageId,Title,ImageFile")] UserPhoto imageModel)
        {
            if (ModelState.IsValid)
            {
                //Save image to wwwroot/image
                string wwwRootPath = _appEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(imageModel.ImageFile.FileName);
                string extension = Path.GetExtension(imageModel.ImageFile.FileName);
                imageModel.Name=fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                string path = Path.Combine(wwwRootPath + "/UserPhotos/", fileName);
                using (var fileStream = new FileStream(path,FileMode.Create))
                {
                    await imageModel.ImageFile.CopyToAsync(fileStream);
                }
                //Insert record
                // _context.Add(imageModel);
                // await _context.SaveChangesAsync();
                // return RedirectToAction(nameof(Index));
            }
            return Page();
        }

        public JsonResult OnGetUserProfile()
        {
            InitCurrentUser();
            var profiles = MyDatabase.GetAllProfiles().Result;
            var profile = profiles.FirstOrDefault(p => p.Id == CurrentUser.Id);

            var result = JsonSerializer.Serialize(profile);
            return new JsonResult(result);
            
            // else
            // {
            //     return new JsonResult("error");
            // }
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