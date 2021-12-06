using System;
using System.Collections.Generic;
using System.Linq;
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
        private ApplicationDbContext _db;

        public IndexModel(ILogger<IndexModel> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _db = context;
        }

        public void OnGet()
        {
        }
        
        public async Task<IActionResult> OnPost(string email, string username, string password)
        {
            var newUser = new User()
            {
                Id = Guid.NewGuid(),
                Email = email,
                Username = username,
                Password = password
            };
            await MyDatabase.Add(newUser);
            return Redirect("/privacy");
        }
    }
}