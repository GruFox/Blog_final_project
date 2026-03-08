namespace Blog_final_project.Models;

public class DetailsArticleViewModel
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Text { get; set; } = null!;
    public int AuthorId { get; set; }
    public List<string> Tags { get; set; } = new();
    public List<CommentItemViewModel> Comments { get; set; } = new();
    public string NewCommentText { get; set; } = null!;
    public int ArticleId { get; set; }
    public string AuthorName { get; set; } = null!;
}
