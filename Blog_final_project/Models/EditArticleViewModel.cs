using System.ComponentModel.DataAnnotations;

namespace Blog_final_project.Models;

public class EditArticleViewModel
{
    public int Id { get; set; }
    [Required]
    [Display(Name = "Заголовок")]
    public string Title { get; set; } = null!;
    [Required]
    [Display(Name = "Контекст")]
    public string Text { get; set; } = null!;
    public List<Tag> AllTags { get; set; } = new();
    public List<int> SelectedTagIds { get; set; } = new();
}
