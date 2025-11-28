using Microsoft.EntityFrameworkCore;

namespace BeanPriceViewer.Models
{
    public class CityDbContext : DbContext
    {
        public DbSet<City> CitySet { get; set; }

        public CityDbContext(DbContextOptions<CityDbContext> options)
            : base(options)
        {
            
        }
    }
}
