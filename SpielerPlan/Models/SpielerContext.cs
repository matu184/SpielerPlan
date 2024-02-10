using Microsoft.EntityFrameworkCore;

namespace SpielerPlan.Models
{
    public class SpielerContext : DbContext
    {
        public SpielerContext(DbContextOptions<SpielerContext> options)
            : base(options)
        {
        }

        public DbSet<Spieler> Spieler { get; set; }
    }
}
