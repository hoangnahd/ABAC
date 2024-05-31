using ABAC.Models;
using Microsoft.EntityFrameworkCore;

namespace ABAC.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Resource> Resources { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed default data
            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, Username = "Alice", Role = "manager", Department = "sales" }
            );

            modelBuilder.Entity<Resource>().HasData(
                new Resource { Id = 1, Name = "file1", Type = "report", Owner = "Alice" }
            );
        }
    }
}
