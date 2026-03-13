using Blog_final_project.Models;

namespace Blog_final_project.Interfaces;

public interface IRoleRepository
{
    /// <summary>
    /// Получает список всех ролей
    /// </summary>
    /// <returns>Список ролей</returns>
    Task<List<Role>> GetAllRolesAsync();

    /// <summary>
    /// Создает роль
    /// </summary>
    /// <param name="name">Название новой роли</param>
    /// <param name="description">Описание новой роли</param>
    /// <returns></returns>
    Task CreateRoleAsync(string name, string description);

    /// <summary>
    /// Проверяет на совпадение названий роли
    /// </summary>
    /// <param name="name">Название роли</param>
    /// <returns>true or false</returns>
    Task<bool> RoleExistsAsync(string name);

    /// <summary>
    /// Получает роль по Id
    /// </summary>
    /// <param name="id">Идентификатор роли</param>
    /// <returns>Роль</returns>
    Task<Role?> GetRoleById(int id);
    
    /// <summary>
    /// Сохраняет изменения роли
    /// </summary>
    /// <returns></returns>
    Task SaveAsync();
}
