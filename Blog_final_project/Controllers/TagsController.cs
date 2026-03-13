using System.Security.Claims;
using Blog_final_project.Interfaces;
using Blog_final_project.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog_final_project.Controllers;

[Authorize]
public class TagsController : Controller
{

    private readonly ITagRepository _tagRepository;
    private readonly ILogger<TagsController> _logger;

    public TagsController(ITagRepository tagRepository, ILogger<TagsController> logger)
    {

        _tagRepository = tagRepository;
        _logger = logger;
    }

    /// <summary>
    /// Отображает список всех тегов
    /// </summary>
    /// <returns>Представление со списком всех тегов</returns>
    public async Task<IActionResult> Index()
    {
        var tags = await _tagRepository.ShowTagsAsync();

        var model = new TagViewModel
        {
            Tags = tags
        };

        return View(model);
    }

    /// <summary>
    /// Отображает форму создания нового тега
    /// </summary>
    /// <returns>Представление с формой создания нового тега</returns>
    /// <remarks>
    /// Доступ к методу имеют только пользователи с ролями Admin или Moderator
    /// </remarks>
    [Authorize(Roles = "Admin,Moderator")]
    public IActionResult Create()
    {
        var model = new CreateTagViewModel();

        return View(model);
    }

    /// <summary>
    /// Сохраняет созданный тег
    /// </summary>
    /// <param name="model">Модель с данными тега</param>
    /// <returns>Перенаправляет на страницу со списком всех тегов</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateTagViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        if (await _tagRepository.TagExistsAsync(model.Name))
        {
            ModelState.AddModelError("Name", "Такой тег уже существует");
            return View(model);
        }

        await _tagRepository.CreateTagAsync(model.Name);

        _logger.LogInformation(
            "User {UserId} created tag {TagName}",
            GetCurrentUserId(),
            model.Name);

        return RedirectToAction(nameof(Index));
    }

    /// <summary>
    /// Отображает форму редактирования выбранного тега
    /// </summary>
    /// <param name="id">Идентификатор тега</param>
    /// <returns>Представление с формой редактирования выбранного тега</returns>
    /// <remarks>
    /// Доступ к методу имеют только пользователи с ролями Admin или Moderator
    /// </remarks>
    [Authorize(Roles = "Admin,Moderator")]
    public async Task<IActionResult> Edit(int id)
    {
        var tag = await _tagRepository.GetTagByIdAsync(id);

        if (tag == null) return NotFound();

        var model = new EditTagViewModel
        {
            Id = tag.Id,
            Name = tag.Name
        };

        return View(model);
    }

    /// <summary>
    /// Сохраняет измененный тег
    /// </summary>
    /// <param name="model">Модель с измененными данными тега</param>
    /// <returns>Перенаправляет на страницу со списком всех тегов</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(EditTagViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        if (await _tagRepository.TagExistsAsync(model.Name))
        {
            ModelState.AddModelError("Name", "Такой тег уже существует");
            return View(model);
        }

        var tag = await _tagRepository.GetTagByIdAsync(model.Id);
        if (tag == null) return NotFound();

        tag.Name = model.Name;

        await _tagRepository.SaveAsync();

        _logger.LogInformation(
            "User {UserId} edited tag {TagId}",
            GetCurrentUserId(),
            tag.Id);

        return RedirectToAction(nameof(Index));
    }

    /// <summary>
    /// Получить текущего пользователя
    /// </summary>
    /// <returns>Идентификатор текущего пользователя</returns>
    /// <exception cref="Exception"></exception>
    private int GetCurrentUserId()
    {
        var claim = User.FindFirst(ClaimTypes.NameIdentifier);

        if (claim == null)
            throw new Exception("User id claim not found");

        return int.Parse(claim.Value);
    }
}
