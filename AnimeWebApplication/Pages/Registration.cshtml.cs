using System;
using System.Threading.Tasks;
using System.Linq;
using AnimeWebApp;
using AnimeWebApplication.Database;
using AnimeWebApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AnimeWebApplication.Pages
{
    public class Registration : PageModel
    {
        private readonly IJWTAuthenticationManager jWTAuthenticationManager;

        public Registration(IJWTAuthenticationManager jWTAuthenticationManager)
        {
            this.jWTAuthenticationManager = jWTAuthenticationManager;
        }
        public void OnGet()
        {
            
        }

        public async Task<IActionResult> OnPost(string email, string username, string password, string confirmPassword)
        {
            if (ModelState.IsValid && password == confirmPassword)
            {
                if (ModelState.IsValid)
                {
                    var users = await MyDatabase.GetAllUsers();
                    var user = users.FirstOrDefault(u => u.Email == email || u.Username == username);
                    if (user == null)
                    {
                        var currentUser = new User
                        {
                            Id = Guid.NewGuid(),
                            Email = email,
                            Password = Encryption.EncryptString(password),
                            Username = username,
                        };
                        await MyDatabase.Add(currentUser);

                        var token = jWTAuthenticationManager.Authenticate(currentUser);
                        Response.Cookies.Append("token", token);
                        RedirectToAction("Index", "Home");

                        return Redirect("/index");
                    }
                    else
                        ModelState.AddModelError("", $"Пользователь с такой почтой или именем пользователя  уже зарегистрирован.");
                }
            }
            else
                ModelState.AddModelError("", $"Не все поля заполнены.");
            return Page();
        }
    }
}