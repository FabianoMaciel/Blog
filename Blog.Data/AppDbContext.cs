using Blog.Data.Configurations;
using Blog.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Blog.Data
{
    public class AppDbContext : IdentityDbContext
    {
        
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new PostConfiguration());
            modelBuilder.ApplyConfiguration(new CommentConfiguration());

            
            modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole
            {
                Id = "1",
                Name = "admin",
                NormalizedName = "ADMIN"
            });

            var identityUser = new IdentityUser
            {
                UserName = "admin@blog.com",
                Email = "admin@blog.com",
                NormalizedEmail = "ADMIN@BLOG.COM",
                NormalizedUserName = "ADMIN@BLOG.COM",
                LockoutEnabled = true,
                EmailConfirmed = true
            };

            var hasher = new PasswordHasher<IdentityUser>();
            identityUser.PasswordHash = hasher.HashPassword(identityUser, "Admin123#");
            modelBuilder.Entity<IdentityUser>().HasData(identityUser);

            modelBuilder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                RoleId = "1",
                UserId = identityUser.Id
            });

            modelBuilder.Entity<User>().HasData(new User { Id = 1, CreatedAt = DateTime.Now, DateOfBirth = DateTime.Now, IsAdmin = true, Name = "Administrator", Occupation = "System Admin", IdentityUserId = identityUser.Id });
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Post> Posts { get; set; }
    }
}
