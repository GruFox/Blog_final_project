namespace Blog_final_project.Models;

public class Comment
{
    public int Id { get; set; }
    public string CommentText { get; set; } = null!;
    public int CommentatorId { get; set; }
    public User Commentator { get; set; } = null!;
    public int ArticleId { get; set; }
    public Article Article { get; set; } = null!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
