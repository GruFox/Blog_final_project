using Blog_final_project.Models;

namespace Blog_final_project.Interfaces;

public interface IUserRepository
{
    Task<List<User>> ShowUsersAsync();
    Task<User?> GetUserByIdAsync(int id);
    Task UpdateUserRolesAsync(User user, List<int> selectedRoleIds);
    Task SaveAsync();
}
