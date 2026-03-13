using Blog_final_project.Data;
using Blog_final_project.Interfaces;
using Blog_final_project.Models;
using Microsoft.EntityFrameworkCore;

namespace Blog_final_project.Repositoties;

public class RoleRepository : IRoleRepository
{
    private readonly BlogDbContext _context;

    public RoleRepository(BlogDbContext context)
    {
        _context = context;
    }

    public async Task<List<Role>> GetAllRolesAsync()
    {
        return await _context.Roles.ToListAsync();
    }

    public async Task CreateRoleAsync(string name, string description)
    {
        var role = new Role
        {
            Name = name,
            Description = description
        };
        _context.Add(role);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> RoleExistsAsync(string name)
    {
        return await _context.Roles.AnyAsync(r => r.Name == name);
    }

    public async Task<Role?> GetRoleById(int id)
    {
        return await _context.Roles
            .Include(r => r.UserRoles)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
}
