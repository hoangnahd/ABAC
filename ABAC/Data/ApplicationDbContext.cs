using ABAC.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace ABAC.Data
{
    public class ApplicationDbContext : IdentityDbContext<User, Roles, int>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<LNK_ROLE_RESOURCES> RoleResources { get; set; }
        public DbSet<LNK_USER_ROLE> UserRole {  get; set; }
        public DbSet<Resource> Resources { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Configure User entity
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasMany(e => e.UserRoles)
                      .WithOne(ur => ur.User)
                      .HasForeignKey(ur => ur.UserId)
                      .IsRequired();
            });

            // Configure Role entity
            modelBuilder.Entity<Roles>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasMany(e => e.UserRoles)
                      .WithOne(ur => ur.Role)
                      .HasForeignKey(ur => ur.RoleId)
                      .IsRequired();
                entity.HasMany(e => e.RoleResources)
                      .WithOne(rr => rr.Role)
                      .HasForeignKey(rr => rr.RoleId)
                      .IsRequired();
            });

            // Configure UserRole entity
            modelBuilder.Entity<LNK_USER_ROLE>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId });
                entity.HasOne(ur => ur.User)
                      .WithMany(u => u.UserRoles)
                      .HasForeignKey(ur => ur.UserId);
                entity.HasOne(ur => ur.Role)
                      .WithMany(r => r.UserRoles)
                      .HasForeignKey(ur => ur.RoleId);
            });

            // Configure Resource entity
            modelBuilder.Entity<Resource>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasMany(e => e.RoleResources)
                      .WithOne(rr => rr.Resource)
                      .HasForeignKey(rr => rr.ResourceId)
                      .IsRequired();
            });

            // Configure RoleResource entity
            modelBuilder.Entity<LNK_ROLE_RESOURCES>(entity =>
            {
                entity.HasKey(e => new { e.RoleId, e.ResourceId });
                entity.HasOne(rr => rr.Role)
                      .WithMany(r => r.RoleResources)
                      .HasForeignKey(rr => rr.RoleId);
                entity.HasOne(rr => rr.Resource)
                      .WithMany(r => r.RoleResources)
                      .HasForeignKey(rr => rr.ResourceId);
            });

            // Seed default data for Roles and Resources
            SeedRoles(modelBuilder);
            SeedResources(modelBuilder);
            SeedAdminUser(modelBuilder);
        }
        private void SeedRoles(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Roles>().HasData(
                new Roles { Id = 1, Name = "HR_Manager" },
                new Roles { Id = 2, Name = "IT_Specialist" },
                new Roles { Id = 3, Name = "Auditor" },
                new Roles { Id = 4, Name = "IT_Admin" },
                new Roles { Id = 5, Name = "HR_Assistant" }
            );
        }

        private void SeedResources(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Resource>().HasData(
                new Resource 
                { 
                    Id = 101, 
                    Type = "Document", 
                    Sensitivity = "High", 
                    Content= "The data type for this entity is a Document with High sensitivity department IT",
                    Department="IT",
                    Owner = [1]
                },
                new Resource 
                { 
                    Id = 102, 
                    Type = "Server", 
                    Sensitivity = "Medium", 
                    Content = "The data type for this entity is a Server with Medium sensitivity department IT",
                    Department = "IT",
                    Owner = [1]
                },
                new Resource 
                { 
                    Id = 103, Type = "Report", 
                    Sensitivity = "Low", 
                    Content = "The data type for this entity is a Report with low sensitivity department Finance",
                    Department = "Finance",
                    Owner = [3]
                },
                new Resource 
                { 
                    Id = 104, 
                    Type = "Network", 
                    Sensitivity = "High", 
                    Content = "The data type for this entity is a Network with High sensitivity department Finance",
                    Department= "Finance",
                    Owner = [3, 4]
                },
                new Resource 
                { 
                    Id = 105, 
                    Type = "Document", 
                    Sensitivity = "High", 
                    Content = "The data type for this entity is a Document with High sensitivity department HR",
                    Department = "HR",
                    Owner = [2]
                },
                new Resource
                {
                    Id = 106,
                    Type = "Document",
                    Sensitivity = "Medium",
                    Content = "The data type for this entity is a Document with Medium sensitivity department HR",
                    Department = "HR",
                    Owner = [2,4]
                }
            );
        }

        private void SeedAdminUser(ModelBuilder modelBuilder)
        {
            var adminUser = new User
            {
                Id = 1, // Assuming you want to set the Id explicitly
                UserName = "admin",
                Department = "IT",
                Email = "admin@example.com", // You may want to provide an email address
                EmailConfirmed = true, // Assuming you want to confirm the email
                PhoneNumber = "1234567890", // You may want to provide a phone number
                PhoneNumberConfirmed = true, // Assuming you want to confirm the phone number
                LockoutEnabled = false, // Assuming you don't want to enable lockout
                AccessFailedCount = 0, // Assuming there have been no failed access attempts
                PasswordHash = new PasswordHasher<User>().HashPassword(null, "admin"),
            };

            // New user
            var newUser = new User
            {
                Id = 2,
                UserName = "quocanh",
                Department = "HR",
                Email = "john@example.com",
                EmailConfirmed = true,
                PhoneNumber = "1234567890", // You may want to provide a phone number
                PhoneNumberConfirmed = true, // Assuming you want to confirm the phone number
                LockoutEnabled = false, // Assuming you don't want to enable lockout
                AccessFailedCount = 0, // Assuming there have been no failed access attemp
                PasswordHash = new PasswordHasher<User>().HashPassword(null, "quocanh"),
            };
            var addEditor = new User
            {
                Id = 3,
                UserName = "anhanh",
                Department = "Finance",
                Email = "anhanh@example.com",
                EmailConfirmed = true,
                PhoneNumber = "1234567890", // You may want to provide a phone number
                PhoneNumberConfirmed = true, // Assuming you want to confirm the phone number
                LockoutEnabled = false, // Assuming you don't want to enable lockout
                AccessFailedCount = 0, // Assuming there have been no failed access attemp
                PasswordHash = new PasswordHasher<User>().HashPassword(null, "anhanh"),
            };
            var addChairman = new User
            {
                Id = 4,
                UserName = "hoanganh",
                sysAdmin = true,
                Email = "hoanganh@example.com",
                EmailConfirmed = true,
                PhoneNumber = "1234567890", // You may want to provide a phone number
                PhoneNumberConfirmed = true, // Assuming you want to confirm the phone number
                LockoutEnabled = false, // Assuming you don't want to enable lockout
                AccessFailedCount = 0, // Assuming there have been no failed access attemp
                PasswordHash = new PasswordHasher<User>().HashPassword(null, "hoanganh"),
            };

            modelBuilder.Entity<User>().HasData(adminUser, newUser, addChairman);

            // User roles
            modelBuilder.Entity<LNK_USER_ROLE>().HasData(
                new LNK_USER_ROLE { UserId = adminUser.Id, RoleId = 4 }, // Assuming IT_Admin role ID is 4
                new LNK_USER_ROLE { UserId = newUser.Id, RoleId = 1 },    // Assuming HR_Manager role ID is 1,
                new LNK_USER_ROLE { UserId = addEditor.Id, RoleId = 3 }

            );

            // Role resources
            modelBuilder.Entity<LNK_ROLE_RESOURCES>().HasData(
                new LNK_ROLE_RESOURCES { RoleId = 4, ResourceId = 101 }, // Admin has access to high-level document
                new LNK_ROLE_RESOURCES { RoleId = 3, ResourceId = 104 },
                new LNK_ROLE_RESOURCES { RoleId = 1, ResourceId = 105 }  // HR_Manager has access to high-level document
                // Add more resources as needed
            );
        }

    }
}
