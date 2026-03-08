using Blog_final_project.Data;
using Blog_final_project.Interfaces;
using Blog_final_project.Models;
using Microsoft.EntityFrameworkCore;

namespace Blog_final_project.Repositoties;

public class CommentRepository : ICommentRepository
{
    private readonly BlogDbContext _context;

    public CommentRepository(BlogDbContext context)
    {
        _context = context;
    }

    public async Task<List<Comment>> GetAllCommentsAsync()
    {
        return await _context.Comments
            .Include(c => c.Article)
            .Include(c => c.Commentator)
            .ToListAsync();
    }

    public async Task<Comment?> GetCommentByIdAsync(int id)
    {
        return await _context.Comments
            .Include(c => c.Article)
            .Include(c => c.Commentator)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task AddCommentAsync(Comment comment)
    {
        await _context.Comments.AddAsync(comment);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateCommentAsync(Comment comment)
    {
        _context.Comments.Update(comment);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteCommentAsync(Comment comment)
    {
        _context.Comments.Remove(comment);
        await _context.SaveChangesAsync();
    }
}
