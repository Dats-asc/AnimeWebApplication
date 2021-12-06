using System;
using System.Threading.Tasks;
using AnimeWebApplication.Database;
using AnimeWebApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AnimeWebApplication.Pages
{
    public class Registration : PageModel
    {
        public void OnGet()
        {
            
        }

        public async Task<IActionResult> OnPost(string email, string username, string password, string confirmPassword)
        {
            var newUser = new User()
            {
                Id = Guid.NewGuid(),
                Email = email,
                Username = username,
                Password = password
            };
            await MyDatabase.Add(newUser);

            return Redirect("/index");
        }
    }
}