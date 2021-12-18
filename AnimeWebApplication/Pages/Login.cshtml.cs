using System.Linq;
using System.Threading.Tasks;
using AnimeWebApp;
using AnimeWebApplication.Database;
using AnimeWebApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AnimeWebApplication.Pages
{
    public class Login : PageModel
    {
        
        private readonly IJWTAuthenticationManager jWTAuthenticationManager;

        public Login(IJWTAuthenticationManager jWTAuthenticationManager)
        {
            this.jWTAuthenticationManager = jWTAuthenticationManager;
        }
        
        
        public void OnGet()
        {
            
        }

        public async Task<IActionResult> OnPost(string email, string password)
        {
            var users = await MyDatabase.GetAllUsers();
            User user = users.FirstOrDefault(u => u.Email == email && u.Password == Encryption.EncryptString(password));
            if (user == null)
            {
                return Redirect("/registration");
            }
            
            var token = jWTAuthenticationManager.Authenticate(user);

            if (token == null)
                return Redirect("/registration");
            else
            {
                Response.Cookies.Append("token", token);
                return Redirect("/index");
            }
        }
    }
}