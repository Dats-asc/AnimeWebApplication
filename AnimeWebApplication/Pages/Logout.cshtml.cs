using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AnimeWebApplication.Pages
{
    public class Logout : PageModel
    {
        public void OnGet()
        {
            
        }
        
        public JsonResult OnGetLogout() // Logout
        {
            var token = Request.Cookies["token"];
            Response.Cookies.Delete("token");
            return new JsonResult("ok");
        }
    }
}