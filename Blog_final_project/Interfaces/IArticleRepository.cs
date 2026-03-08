using Blog_final_project.Models;

namespace Blog_final_project.Interfaces;

public interface IArticleRepository
{
    Task<List<Article>> ShowArticlesAsync();
    Task<Article?> GetArticleById(int id);
    Task CreateArticleAsync(Article article);
    Task SaveAsync();
    Task DeleteAsync(Article article);
    Task<bool> ArticleExistsAsync(string title);
}
