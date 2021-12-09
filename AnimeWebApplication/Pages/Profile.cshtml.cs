using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using AnimeWebApplication.Database;
using AnimeWebApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AnimeWebApplication.Pages
{
    public class Profile : PageModel
    {
        public User CurrentUser;
        public Models.Profile CurrentProfile;
        
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