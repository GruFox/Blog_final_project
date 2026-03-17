using Blog_final_project.Models;

namespace Blog_final_project.Interfaces;

public interface IArticleRepository
{
    /// <summary>
    /// Получает список всех статей
    /// </summary>
    /// <returns>Список статей</returns>
    Task<List<Article>> ShowArticlesAsync();

    /// <summary>
    /// Получает статью по Id
    /// </summary>
    /// <param name="id">Идентификатор статьи</param>
    /// <returns>Статью</returns>
    Task<Article?> GetArticleByIdAsync(int id);

    /// <summary>
    /// Создает статью
    /// </summary>
    /// <param name="article">Название новой статьи</param>
    /// <returns></returns>
    Task CreateArticleAsync(Article article);

    /// <summary>
    /// Сохраняет изменения статьи
    /// </summary>
    /// <returns></returns>
    Task SaveAsync();

    /// <summary>
    /// Удаляет статью
    /// </summary>
    /// <param name="article">Удаляемая статья</param>
    /// <returns></returns>
    Task DeleteArticleAsync(Article article);

    /// <summary>
    /// Проверяет на совпадение названий статьи
    /// </summary>
    /// <param name="title">Название статьи</param>
    /// <returns>true or false</returns>
    Task<bool> ArticleExistsAsync(string title);
}
