using System.ComponentModel.DataAnnotations;

namespace Blog_final_project.Models;

public class EditArticleViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Введите название статьи")]
    [StringLength(100, ErrorMessage = "Название слишком длинное")]
    [Display(Name = "Заголовок")]
    public string Title { get; set; } = null!;

    [Required(ErrorMessage = "Введите текст")]
    [StringLength(10000, ErrorMessage = "Текст статьи слишком длинный")]
    [Display(Name = "Контент")]
    public string Text { get; set; } = null!;

    public List<Tag> AllTags { get; set; } = new();

    public List<int> SelectedTagIds { get; set; } = new();
}
