using Blog_final_project.Models;
using Microsoft.EntityFrameworkCore;

namespace Blog_final_project.Data;

public class SeedData
{
    /// <summary>
    /// Создает роли и пользователей по умолчанию
    /// </summary>
    /// <param name="context">Параметр соединения с БД</param>
    /// <returns></returns>
    public static async Task InitializeAsync(BlogDbContext context)
    {
        // Создание роли
        if (!await context.Roles.AnyAsync())
        {
            context.Roles.AddRange(
                new Role
                {
                    Name = "Admin",
                    Description = "Роль с максимальными возможностями в приложении"
                },
                new Role
                {
                    Name = "Moderator",
                    Description = "Редактирование и удаление комментариев и статей"
                },
                new Role
                {
                    Name = "User",
                    Description = "Стандартная роль приложения"
                }
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
