using Blog_final_project.Models;
using Microsoft.EntityFrameworkCore;

namespace Blog_final_project.Data;

public class SeedData
{
    public static async Task InitializeAsync(BlogDbContext context)
    {
        // Создание роли
        if (!await context.Roles.AnyAsync())
        {
            context.Roles.AddRange(
                new Role { Name = "Admin" },
                new Role { Name = "Moderator" },
                new Role { Name = "User" }
            );

            await context.SaveChangesAsync();
        }

        // Создание пользователей
        if (!await context.Users.AnyAsync())
        {
            var admin = new User
            {
                UserName = "admin",
                Email = "admin@mail.com",
                Password = "admin"
            };

            var moderator = new User
            {
                UserName = "moderator",
                Email = "moderator@mail.com",
                Password = "moderator"
            };

            var user = new User
            {
                UserName = "user",
                Email = "user@mail.com",
                Password = "user"
            };

            context.Users.AddRange(admin, moderator, user);
            await context.SaveChangesAsync();

            // Получение ролей
            var adminRole = await context.Roles.FirstAsync(r => r.Name == "Admin");
            var moderatorRole = await context.Roles.FirstAsync(r => r.Name == "Moderator");
            var userRole = await context.Roles.FirstAsync(r => r.Name == "User");

            // Назначение роли
            context.UserRoles.AddRange(
                new UserRole { UserId = admin.Id, RoleId = adminRole.Id },
                new UserRole { UserId = moderator.Id, RoleId = moderatorRole.Id },
                new UserRole { UserId = user.Id, RoleId = userRole.Id });

            await context.SaveChangesAsync();
        }
    }
}
