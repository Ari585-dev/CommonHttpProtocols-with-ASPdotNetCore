using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;


namespace WebApplication1
{
    public class DbConfig : DbContext
    {
        public DbConfig(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Company> Companies { get; set; }
        public DbSet<Game> Games { get; set; }
    }
}
