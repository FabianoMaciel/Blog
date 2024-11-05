using Core.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Core.Seed;

public static class DbMigrationHelperExtension
{
    public static void UseDbMigrationHelper(this WebApplication app)
    {
        DbMigrationHelper.EnsureSeedData(app).Wait();
    }
}
public static class DbMigrationHelper
{
    public static async Task EnsureSeedData(WebApplication application)
    {
        var services = application.Services.CreateScope().ServiceProvider;
        await EnsureSeedData(services);
    }

    public static async Task EnsureSeedData(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
        var env = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();

        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        if (env.IsDevelopment())
        {
            await context.Database.MigrateAsync();

            await EnsureSeedTables(context);
        }
    }

    private static async Task EnsureSeedTables(AppDbContext context)
    {
        if (context.Posts.Any() || context.Users.Any()) return;

        var hasher = new PasswordHasher<IdentityUser>();

        var admin = new IdentityUser
        {
            Id = "1",
            Email = "admin@blog.com",
            EmailConfirmed = true,
            NormalizedEmail = "ADMIN@BLOG.COM",
            UserName = "admin@blog.com",
            AccessFailedCount = 0,
            NormalizedUserName = "ADMIN@BLOG.COM"
        };

        admin.PasswordHash = hasher.HashPassword(admin, "Admin123#");

        var user2 = new IdentityUser
        {
            Id = "2",
            Email = "fabiano@blog.com",
            EmailConfirmed = true,
            NormalizedEmail = "FABIANO@BLOG.COM",
            UserName = "fabiano@blog.com",
            AccessFailedCount = 0,
            NormalizedUserName = "FABIANO@BLOG.COM"
        };

        user2.PasswordHash = hasher.HashPassword(user2, "User2#");

        var user3 = new IdentityUser
        {
            Id = "3",
            Email = "nelson@blog.com",
            EmailConfirmed = true,
            NormalizedEmail = "NELSON@BLOG.COM",
            UserName = "nelson@blog.com",
            AccessFailedCount = 0,
            NormalizedUserName = "NELSON@BLOG.COM",
        };

        user3.PasswordHash = hasher.HashPassword(user3, "User3#");

        context.Roles.Add(
            new IdentityRole
            {
                Id = "1",
                Name = "admin",
                NormalizedName = "ADMIN"
            });

        context.UserRoles.Add(new IdentityUserRole<string>
        {
            RoleId = "1",
            UserId = admin.Id
        });


        await context.Users.AddAsync(admin);
        var authorAdmin = new Entities.Author { User = admin };
        await context.Authors.AddAsync(authorAdmin);
        await context.Users.AddAsync(user2);
        var authorUser2 = new Entities.Author { User = user2 };
        await context.Authors.AddAsync(authorUser2);
        var authorUser3 = new Entities.Author { User = user3 };
        await context.Users.AddAsync(user3);
        await context.Authors.AddAsync(authorUser3);

        var post = new Post
        {
            Title = "What is this blog about?",
            Content =
          "This blog is the first project needed for the MBA of Full Stack .NET Expert, and that is why it has been developed with much love and attetion",
            Author = admin,
            CreatedAt = DateTime.Now,
            Comments = new List<Comment>
            {
                new() {
                    CreatedAt = DateTime.Now,
                    Content = "Pontos Positivos: x1, x2, x3",
                    AuthorId = user2.Id
                },
                new() {
                    CreatedAt = DateTime.Now,
                    Content = "Pontos Negativos: y1, y2, y3",
                    AuthorId = user3.Id
                }
            }
        };

        await context.Posts.AddAsync(post);

        await context.SaveChangesAsync();
    }
}
