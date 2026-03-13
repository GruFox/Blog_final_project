using Blog_final_project.Interfaces;
using Blog_final_project.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog_final_project.Controllers;

[Authorize]
public class RolesController : Controller
{
    private readonly IRoleRepository _roleRepository;
    private readonly ILogger<RolesController> _logger;


    public RolesController(IRoleRepository roleRepository, ILogger<RolesController> logger)
    {
        _roleRepository = roleRepository;
        _logger = logger;
    }

    /// <summary>
    /// Отображает список всех ролей
    /// </summary>
    /// <returns>Представление со списком всех ролей</returns>
    public async Task<IActionResult> Index()
    {
        var roles = await _roleRepository.GetAllRolesAsync();
        
        var model = new RoleViewModel()
        {
            Roles = roles
        };

        return View(model);
    }

    /// <summary>
    /// Отображает подробные данные выбранной роли
    /// </summary>
    /// <param name="id">Идентификатор роли</param>
    /// <returns>Представление с подробными данными выбранной роли</returns>
    public async Task<IActionResult> Details(int id)
    {
        var role = await _roleRepository.GetRoleById(id);

        if (role == null)
            return NotFound();

        var model = new DetailsRoleViewModel
        {
            Id = role.Id,
            Name = role.Name,
            Description = role.Description
        };

        return View(model);
    }

    /// <summary>
    /// Отображает форму создания новой роли
    /// </summary>
    /// <returns>Представление с формой создания новой роли</returns>
    /// <remarks>
    /// Доступ к методу имеет только пользователь с ролью Admin
    /// </remarks>
    [Authorize(Roles = "Admin")]
    public IActionResult Create()
    {
        var model = new CreateRoleViewModel();

        return View(model);
    }

    /// <summary>
    /// Сохраняет созданную роль
    /// </summary>
    /// <param name="model">Модель с данными роли</param>
    /// <returns>Перенаправляет на страницу со списком всех ролей</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateRoleViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        if (await _roleRepository.RoleExistsAsync(model.Name))
        {
            ModelState.AddModelError("Name", "Такая роль уже существует");
            return View(model);
        }

        await _roleRepository.CreateRoleAsync(model.Name, model.Description);

        _logger.LogInformation(
            "Role {RoleName} created",
            model.Name);

        return RedirectToAction(nameof(Index));
    }

    /// <summary>
    /// Отображает форму редактирования выбранной роли
    /// </summary>
    /// <param name="id">Идентификатор роли</param>
    /// <returns>Представление с формой редактирования выбранной роли</returns>
    /// <remarks>
    /// Доступ к методу имеет только пользователь с ролью Admin
    /// </remarks>
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(int id)
    {
        var role = await _roleRepository.GetRoleById(id);

        if (role == null) return NotFound();

        var model = new EditRoleViewModel
        {
            Id = role.Id,
            Name = role.Name,
            Description = role.Description
        };

        return View(model);
    }

    /// <summary>
    /// Сохраняет измененную роль
    /// </summary>
    /// <param name="model">Модель с измененными данными роли</param>
    /// <returns>Перенаправляет на страницу со списком всех ролей</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(EditRoleViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var role = await _roleRepository.GetRoleById(model.Id);
        if (role == null) return NotFound();

        role.Name = model.Name;
        role.Description = model.Description;

        await _roleRepository.SaveAsync();

        _logger.LogInformation(
            "Role {RoleId} edited",
            role.Id);

        return RedirectToAction(nameof(Index));
    }
}
