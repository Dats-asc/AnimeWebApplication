using System;
using System.Threading.Tasks;
using AnimeWebApplication.Database;
using AnimeWebApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AnimeWebApplication.Pages
{
    public class AddAnime : PageModel
    {
        public void OnGet()
        {
            
        }

        public async Task<JsonResult> OnPostAddAnime(
            [FromForm] string title, 
            [FromForm] string description
            )
        {
            var newAnime = new Anime()
            {
                AnimeId = Guid.NewGuid(),
                Title = title,
                Description = description
            };

            await MyDatabase.Add(newAnime);

            return new JsonResult("ok");
        }
    }
}