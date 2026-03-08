using Blog_final_project.Models;

namespace Blog_final_project.Interfaces;

public interface IRoleRepository
{
    Task<List<Role>> ShowRolesAsync();
    Task CreateRoleAsync(string name, string description);
    Task<bool> RoleExistsAsync(string name);
    Task<Role?> GetRoleById(int id);
    Task<List<Role>> GetAllRolesAsync();
    Task SaveAsync();
}
