using Microsoft.EntityFrameworkCore;

namespace DealerShip.Models
{
    public class DealerShipContext : DbContext
    {
        public DealerShipContext(DbContextOptions<DealerShipContext> options) : base(options)
        {
        }
        public DbSet<Car> Cars { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Link> Links { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            modelBuilder.Seed();
        }
    
    }
}
