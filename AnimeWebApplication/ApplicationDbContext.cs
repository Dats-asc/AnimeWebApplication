using AnimeWebApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace AnimeWebApplication
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}