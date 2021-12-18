using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AnimeWebApplication.Database;
using AnimeWebApplication.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AnimeWebApplication.Pages
{
    public class Test : PageModel
    {
        IWebHostEnvironment _appEnvironment;

        public Test(IWebHostEnvironment appEnvironment)
        {
            _appEnvironment = appEnvironment;
        }
        public void OnGet()
        {
            
        }

        public async Task<RedirectToActionResult> OnPostUploadImage(IFormFile uploadedFile)
        {
            var currentUser = FindUserByToken();
            if (uploadedFile != null)
            {
                // путь к папке Files
                string path = "/UserPhotos/"+ currentUser.Id.ToString() + uploadedFile.FileName;
                // сохраняем файл в папку Files в каталоге wwwroot
                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(fileStream);
                }
            }
            
            return RedirectToAction("/index");
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
    }
    
    
}