using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using AnimeWebApplication.Database;
using AnimeWebApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AnimeWebApplication.Pages
{
    public class UserProfile : PageModel
    {
        public User CurrentUser;
        public Models.Profile CurrentProfile;
        
        public void OnGet()
        {
            
        }

        public async Task<JsonResult> OnGetProfile()
        {
            InitCurrentUser();
            InitProfile();

            var profileModel = new ProfileModel()
            {
                Email = CurrentUser.Email,
                Username = CurrentUser.Username,
                PhotoPath = CurrentProfile.PhotoPath,
                City = CurrentProfile.City,
                Birthday = CurrentProfile.Birthday,
                Description = CurrentProfile.Description
            };

            var result = JsonSerializer.Serialize<ProfileModel>(profileModel);
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