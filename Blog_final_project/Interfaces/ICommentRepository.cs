using Blog_final_project.Models;

namespace Blog_final_project.Interfaces;

public interface ICommentRepository
{
    /// <summary>
    /// Получает список всех комментариев
    /// </summary>
    /// <returns>Список комментариев</returns>
    Task<List<Comment>> GetAllCommentsAsync();

    /// <summary>
    /// Получает комментарий по Id
    /// </summary>
    /// <param name="id">Идентификатор комментария</param>
    /// <returns>Комментарий</returns>
    Task<Comment?> GetCommentByIdAsync(int id);

    /// <summary>
    /// Добавляет новый комментарий
    /// </summary>
    /// <param name="comment">Новый комментарий</param>
    /// <returns></returns>
    Task AddCommentAsync(Comment comment);

    /// <summary>
    /// Обновляет комментарий
    /// </summary>
    /// <param name="comment">Обновляемый комментарий</param>
    /// <returns></returns>
    Task UpdateCommentAsync(Comment comment);

    /// <summary>
    /// Удаляет комментарий
    /// </summary>
    /// <param name="comment">Удаляемый комментарий</param>
    /// <returns></returns>
    Task DeleteCommentAsync(Comment comment);
}
