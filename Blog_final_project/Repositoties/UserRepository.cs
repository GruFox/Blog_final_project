using Blog_final_project.Data;
using Blog_final_project.Interfaces;
using Blog_final_project.Models;
using Microsoft.EntityFrameworkCore;

namespace Blog_final_project.Repositoties;

public class UserRepository : IUserRepository
{
    private readonly BlogDbContext _context;

    public UserRepository(BlogDbContext context)
    {
        _context = context;
    }

    public async Task<List<User>> ShowUsersAsync()
    {
        return await _context.Users
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .ToListAsync();
    }

    public async Task<User?> GetUserByIdAsync(int id)
    {
        return await _context.Users
            .Include(u => u.Articles)
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task UpdateUserRolesAsync(User user, List<int> selectedRoleIds)
    {
        // Загружаем текущие роли пользователя
        var currentUserRoles = await _context.UserRoles
            .Where(ur => ur.UserId == user.Id)
            .ToListAsync();

        // Удаляем старые роли
        _context.UserRoles.RemoveRange(currentUserRoles);

        // Добавляем новые роли
        if (selectedRoleIds != null && selectedRoleIds.Any())
        {
            var newUserRoles = selectedRoleIds.Select(roleId => new UserRole
            {
                UserId = user.Id,
                RoleId = roleId
            });

            await _context.UserRoles.AddRangeAsync(newUserRoles);
        }
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
}
