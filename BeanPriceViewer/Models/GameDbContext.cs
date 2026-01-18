using Microsoft.EntityFrameworkCore;

namespace BeanPriceViewer.Models
{
    public class GameDbContext : DbContext
    {
        public DbSet<GameData> Games { get; set; }

        public GameDbContext(DbContextOptions<GameDbContext> options)
            : base(options)
        {
            
        }
    }
}
