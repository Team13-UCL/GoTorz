using Microsoft.EntityFrameworkCore;
using GoTorz.Model;
namespace GoTorz
{
    

    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Define your DbSets (tables) here
        public DbSet<Package> Package { get; set; }
    }

    
}
