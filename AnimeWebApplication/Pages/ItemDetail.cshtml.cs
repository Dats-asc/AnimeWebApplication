using System;
using System.Linq;
using System.Text.Json;
using AnimeWebApplication.Database;
using AnimeWebApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AnimeWebApplication.Pages
{
    public class ItemDetail : PageModel
    {
        public void OnGet()
        {
            
        }

        public JsonResult OnGetItemDetails()
        {
            if (Request.Cookies["itemId"] != null)
            {
                var items = MyDatabase.GetAllAnimeItems().Result;
                var itemId = Request.Cookies["itemId"];
                var item = items.FirstOrDefault(i => i.ItemId == Guid.Parse(itemId));
                var result = JsonSerializer.Serialize<AnimeItem>(item);
                return new JsonResult(result);
            }
            else
            {
                return new JsonResult("not found");
            }
        }
    }
}