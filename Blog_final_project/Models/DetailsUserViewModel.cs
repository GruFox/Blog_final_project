namespace Blog_final_project.Models;

public class DetailsUserViewModel
{
    public int Id { get; set; }
    public string UserName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? About { get; set; }
    public string? AvatarUrl { get; set; }
    public List<ArticleItemViewModel> Articles { get; set; } = new();
    public List<string> Roles { get; set; } = new();
}
