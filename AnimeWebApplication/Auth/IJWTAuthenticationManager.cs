using AnimeWebApplication.Models;

namespace AnimeWebApp
{
    public interface IJWTAuthenticationManager
    {
        public string Authenticate(User model);
    }
}