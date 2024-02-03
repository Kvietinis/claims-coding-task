using Claims.Persistance.Abstractions.Models;
using Microsoft.EntityFrameworkCore;

namespace Claims.Persistance.Sql
{
    public class ClaimsSqlDbContext : DbContext
    {
        public ClaimsSqlDbContext(DbContextOptions<ClaimsSqlDbContext> options) : base(options)
        {
        }

        public virtual DbSet<Claim> Claims { get; set; }

        public virtual DbSet<Cover> Covers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Claim>(e =>
            {
                e.HasKey(e => e.Id);
            });

            modelBuilder.Entity<Cover>(e =>
            {
                e.HasKey(e => e.Id);
            });
        }
    }
}
