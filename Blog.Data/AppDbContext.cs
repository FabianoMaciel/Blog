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

            const string ADMIN_ID = "a18be9c0-aa65-4af8-bd17-00bd9344e575";
            var hasher = new PasswordHasher<IdentityUser>();
            modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole
            {
                Id = "1",
                Name = "admin",
                NormalizedName = "ADMIN"
            });

            var identityUser = new IdentityUser
            {
                UserName = "Admin",
                Email = "admin@blog.com",
                NormalizedEmail = "admin@blog.com",
                EmailConfirmed = true,
                PasswordHash = hasher.HashPassword(null, "Admin123#") ,
            };
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
