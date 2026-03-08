using Blog_final_project.Models;

namespace Blog_final_project.Interfaces;

public interface ICommentRepository
{
    Task<List<Comment>> GetAllCommentsAsync();
    Task<Comment?> GetCommentByIdAsync(int id);
    Task AddCommentAsync(Comment comment);
    Task UpdateCommentAsync(Comment comment);
    Task DeleteCommentAsync(Comment comment);
}
