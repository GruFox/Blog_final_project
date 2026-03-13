using Blog_final_project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Blog_final_project.Interfaces;

namespace Blog_final_project.Controllers;

[Authorize]
public class UsersController : Controller
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly ILogger<UsersController> _logger;

    public UsersController(IUserRepository userRepository, IRoleRepository roleRepository, ILogger<UsersController> logger)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _logger = logger;
    }

    /// <summary>
    /// Отображает список всех пользователей
    /// </summary>
    /// <returns>Представление со списком всех пользователей</returns>
    public async Task<IActionResult> Index()
    {
        var users = await _userRepository.ShowUsersAsync();

        if (users == null)
            return NotFound();

        var model = new UserViewModel()
        {
            Users = users
        };

        return View(model);
    }

    /// <summary>
    /// Отображает подробные данные выбранного пользователя
    /// </summary>
    /// <param name="id">Идентификатор пользователя</param>
    /// <returns>Представление с данными выбранного пользователя</returns>
    public async Task<IActionResult> Details(int id)
    {
        var user = await _userRepository.GetUserByIdAsync(id);

        if (user == null) return NotFound();

        var model = new DetailsUserViewModel
        {
            Id = user.Id,
            UserName = user.UserName,
            Email = user.Email,
            About = user.About,
            AvatarUrl = user.AvatarUrl,
            Articles = user.Articles
            .Select(a => new ArticleItemViewModel
            {
                Id = a.Id,
                Title = a.Title
            }).ToList()
        };

        return View(model);
    }

    /// <summary>
    /// Отображает форму редактирования выбранного пользователя
    /// </summary>
    /// <param name="id">Идентификатор пользователя</param>
    /// <returns>Представление с формой редактирования выбранного пользователя</returns>
    [Authorize]
    public async Task<IActionResult> Edit(int id)
    {
        var user = await _userRepository.GetUserByIdAsync(id);
        if (user == null) return NotFound();

        var currentUserName = User.Identity!.Name;

        var isAdmin = User.IsInRole("Admin");
        var isOwner = user.UserName == currentUserName;

        if (!isAdmin && !isOwner)
            return Forbid();

        string[] fullName = user.UserName.Split([' ']);

        string firstName = fullName[0];
        string lastName = fullName.Length > 1 ? fullName[1] : string.Empty;

        var allRoles = await _roleRepository.GetAllRolesAsync();

        var model = new EditUserViewModel
        {
            Id = user.Id,
            Roles = allRoles,
            SelectedRoleIds = user.UserRoles.Select(ur => ur.RoleId).ToList(),
            CanEditRoles = isAdmin,
            RegistrationValues = new RegisterViewModel
            {
                FirstName = firstName,
                LastName = lastName,
                EmailReg = user.Email,
                PasswordReg = user.Password
            }
        };

        return View(model);
    }

    /// <summary>
    /// Сохраняет изменения пользователя
    /// </summary>
    /// <param name="model">Модель с измененными данными пользователя</param>
    /// <returns>Перенаправляет на страницу просмотра пользователя</returns>
    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(EditUserViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var user = await _userRepository.GetUserByIdAsync(model.Id);
        if (user == null) return NotFound();

        var currentUserName = User.Identity!.Name;

        var isAdmin = User.IsInRole("Admin");
        var isOwner = user.UserName == currentUserName;

        if (!isAdmin && !isOwner)
            return Forbid();

        // Обновляем обычные поля
        user.UserName = model.RegistrationValues.FirstName + " " + model.RegistrationValues.LastName;
        user.Email = model.RegistrationValues.EmailReg;
        user.Password = model.RegistrationValues.PasswordReg;

        // Роли меняет только админ
        if (isAdmin)
        {
            await _userRepository.UpdateUserRolesAsync(user, model.SelectedRoleIds);
        }

        await _userRepository.SaveAsync();

        _logger.LogInformation(
            "User {UserName} edited User {UserId}",
            currentUserName,
            user.Id);

        return RedirectToAction("Details", new { id = user.Id });
    }
}
