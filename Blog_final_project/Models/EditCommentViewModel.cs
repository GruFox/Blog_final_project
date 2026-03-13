using System.ComponentModel.DataAnnotations;

namespace Blog_final_project.Models;

public class EditCommentViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Введите комментарий")]
    [StringLength(500, ErrorMessage = "Комментарий слишком длинный")]
    public string CommentText { get; set; } = null!;
}