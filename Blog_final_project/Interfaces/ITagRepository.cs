using Blog_final_project.Models;

namespace Blog_final_project.Interfaces;

public interface ITagRepository
{
    /// <summary>
    /// Получает список всех тегов
    /// </summary>
    /// <returns>Список тегов</returns>
    Task<List<Tag>> ShowTagsAsync();

    /// <summary>
    /// Создает тег
    /// </summary>
    /// <param name="name">Название нового тега</param>
    /// <returns></returns>
    Task CreateTagAsync(string name);

    /// <summary>
    /// Проверяет на совпадение названий тега
    /// </summary>
    /// <param name="name">Название тега</param>
    /// <returns>true or false</returns>
    Task<bool> TagExistsAsync(string name);

    /// <summary>
    /// Получает список тегов по выбранным Id
    /// </summary>
    /// <param name="selectedTagIds">Список идентификаторов тега</param>
    /// <returns>Список тегов</returns>
    Task<List<Tag>> GetTagsByIdsAsync(List<int> selectedTagIds);

    /// <summary>
    /// Получает тег по Id
    /// </summary>
    /// <param name="id">Идентификатор тега</param>
    /// <returns>Тег</returns>
    Task<Tag?> GetTagByIdAsync(int id);

    /// <summary>
    /// Сохраняет изменения тега
    /// </summary>
    /// <returns></returns>
    Task SaveAsync();
}
