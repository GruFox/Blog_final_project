using Microsoft.EntityFrameworkCore;

namespace Blog_final_project.Models;

public class Role
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;

    [Comment("Описание роли")]
    public string Description { get; set; } = String.Empty;
    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}
