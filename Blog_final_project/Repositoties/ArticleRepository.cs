using Blog_final_project.Data;
using Blog_final_project.Interfaces;
using Blog_final_project.Models;
using Microsoft.EntityFrameworkCore;

namespace Blog_final_project.Repositoties;

public class ArticleRepository : IArticleRepository
{
    private readonly BlogDbContext _context;

    public ArticleRepository(BlogDbContext context)
    {
        _context = context;
    }

    public async Task<List<Article>> ShowArticlesAsync()
    {
        return await _context.Articles
            .Include(a => a.Author)
            .ToListAsync();
    }

    public async Task<Article?> GetArticleById(int id)
    {
        return await _context.Articles
            .Include(a => a.Tags)
            .Include(a => a.Comments)
            .ThenInclude(c => c.Commentator)
            .Include(a => a.Author)
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task CreateArticleAsync(Article article)
    {        
        _context.Articles.Add(article);
        await _context.SaveChangesAsync();
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Article article)
    {
        _context.Articles.Remove(article);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ArticleExistsAsync(string title)
    {
        return await _context.Articles.AnyAsync(a => a.Title == title);
    }
}
