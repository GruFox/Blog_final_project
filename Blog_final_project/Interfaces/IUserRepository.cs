using Blog_final_project.Models;

namespace Blog_final_project.Interfaces;

public interface IUserRepository
{
    /// <summary>
    /// Получает список всех пользователей
    /// </summary>
    /// <returns>Список пользователей</returns>
    Task<List<User>> ShowUsersAsync();

    /// <summary>
    /// Получает пользователя по Id
    /// </summary>
    /// <param name="id">Идентификатор пользователя</param>
    /// <returns>Пользователя</returns>
    Task<User?> GetUserByIdAsync(int id);

    /// <summary>
    /// Обновляет пользовательские роли
    /// </summary>
    /// <param name="user">Пользователь</param>
    /// <param name="selectedRoleIds">Список ролей</param>
    /// <returns></returns>
    Task UpdateUserRolesAsync(User user, List<int> selectedRoleIds);

    /// <summary>
    /// Сохраняет изменения пользователя
    /// </summary>
    /// <returns></returns>
    Task SaveAsync();
}
