using Microsoft.EntityFrameworkCore;
using SoberProgress.Domain;

namespace SoberProgress.DataAccess
{
    public class AlcoDbContext : DbContext
    {
        public DbSet<AlcoUser> AlcoUsers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=sober.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AlcoUser>().HasKey(u => u.Id);
        }
    }
}
