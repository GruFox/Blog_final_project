using System.ComponentModel.DataAnnotations;

namespace Blog_final_project.Models;

public class CreateCommentViewModel
{
    [Required(ErrorMessage = "Введите комментарий")]
    [StringLength(500, ErrorMessage = "Комментарий слишком длинный")]
    public string CommentText { get; set; } = null!;
    public int ArticleId { get; set; }
}