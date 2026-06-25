using Microsoft.EntityFrameworkCore;
using GameBacklog.Models;

namespace GameBacklog.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }

        public DbSet<Game> Games { get; set; }
        public DbSet<Platform> Platforms {get; set;}
    }
}