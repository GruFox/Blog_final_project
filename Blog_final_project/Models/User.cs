namespace Blog_final_project.Models;

public class User
{
    public int Id { get; set; }
    public string UserName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? About {  get; set; }
    public string? AvatarUrl { get; set; }
    public string Password { get; set; } = null!;
    public ICollection<Article> Articles { get; set; } = new List<Article>();
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}
