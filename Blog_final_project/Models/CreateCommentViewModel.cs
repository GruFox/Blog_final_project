namespace Blog_final_project.Models;

public class CreateCommentViewModel
{
    public string CommentText { get; set; } = null!;
    public int ArticleId { get; set; }
}