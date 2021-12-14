using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using AnimeWebApplication.Database;
using AnimeWebApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace AnimeWebApplication.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        
        public User CurrentUser;

        public List<Anime> AllAnime;
        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public PageResult OnGet()
        {
            InitCurrentUser();
            AllAnime = MyDatabase.GetAllAnimeItems().Result;
            return Page();
        }

        public JsonResult OnGetUsers()
        {
            var dbUsers = MyDatabase.GetAllUsers().Result;
            var users = new Dictionary<string, List<User>>();
            users.Add("users", dbUsers);
            var result = JsonSerializer.Serialize<Dictionary<string, List<User>>>(users);
            return new JsonResult(result);
        }

        public RedirectResult OnPost() // Logout
        {
            var token = Request.Cookies["token"];
            if (token == null)
                return Redirect("/index");
            Response.Cookies.Delete("token");
            return Redirect("/index");
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

        public void InitCurrentUser()
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
    }
}