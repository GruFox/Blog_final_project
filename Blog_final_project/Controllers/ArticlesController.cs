using Blog_final_project.Interfaces;
using Blog_final_project.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Blog_final_project.Controllers;

public class ArticlesController : Controller
{
    private readonly IArticleRepository _articleRepository;
    private readonly ITagRepository _tagRepository;
    private readonly ILogger<ArticlesController> _logger;

    public ArticlesController(IArticleRepository articleRepository, ITagRepository tagRepository, ILogger<ArticlesController> logger)
    {
        _articleRepository = articleRepository;
        _tagRepository = tagRepository;
        _logger = logger;
    }

    /// <summary>
    /// Отображает список всех статей
    /// </summary>
    /// <returns>Представление со списком всех статей</returns>
    public async Task<IActionResult> Index()
    {
        var articles = await _articleRepository.ShowArticlesAsync();

        var model = new ArticleViewModel
        {
            Articles = articles
        };

        return View(model);
    }

    /// <summary>
    /// Отображает подробные данные выбранной статьи
    /// </summary>
    /// <param name="id">Идентификатор статьи</param>
    /// <returns>Представление с подробными данными выбранной статьи</returns>
    public async Task<IActionResult> Details(int id)
    {
        var article = await _articleRepository.GetArticleByIdAsync(id);

        if (article == null)
            return NotFound();

        var model = new DetailsArticleViewModel
        {
            Id = article.Id,
            ArticleId = article.Id,
            Title = article.Title,
            Text = article.Text,
            AuthorId = article.AuthorId,
            AuthorName = article.Author.UserName,

            Tags = article.Tags
            .Select(t => t.Name)
            .ToList(),

            Comments = article.Comments
            .Select(c => new CommentItemViewModel
            {
                Id = c.Id,
                Text = c.CommentText,
                AuthorName = c.Commentator.UserName
            })

            .ToList()
        };

        return View(model);
    }

    /// <summary>
    /// Отображает форму создания новой статьи
    /// </summary>
    /// <returns>Представление с формой создания новой статьи</returns>
    [Authorize]
    public async Task<IActionResult> Create()
    {
        var tags = await _tagRepository.ShowTagsAsync();

        var model = new CreateArticleViewModel
        {
            AllTags = tags
        };

        return View(model);
    }

    /// <summary>
    /// Сохраняет созданную статью
    /// </summary>
    /// <param name="model">Модель с данными статьи</param>
    /// <returns>Перенаправляет на страницу со списком всех статей</returns>
    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateArticleViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.AllTags = await _tagRepository.ShowTagsAsync();
            return View(model);
        }

        var selectedTags = new List<Tag>();

        if (model.SelectedTagIds != null && model.SelectedTagIds.Any())
        {
            selectedTags = await _tagRepository.GetTagsByIdsAsync(model.SelectedTagIds);
        }

        var userId = GetCurrentUserId();

        var article = new Article
        {
            Title = model.Title,
            Text = model.Text,
            AuthorId = userId,
            Tags = selectedTags
        };

        await _articleRepository.CreateArticleAsync(article);

        _logger.LogInformation(
            "User {UserId} created article {ArticleId}",
            userId,
            article.Id);

        return RedirectToAction(nameof(Index));
    }

    /// <summary>
    /// Отображает форму редактирования выбранной статьи
    /// </summary>
    /// <param name="id">Идентификатор статьи</param>
    /// <returns>Представление с формой редактирования выбранной статьи</returns>
    [Authorize]
    public async Task<IActionResult> Edit(int id)
    {
        var article = await _articleRepository.GetArticleByIdAsync(id);

        if (article == null) return NotFound();

        var allTags = await _tagRepository.ShowTagsAsync();

        var model = new EditArticleViewModel
        {
            Id = article.Id,
            Title = article.Title,
            Text = article.Text,
            SelectedTagIds = article.Tags.Select(t => t.Id).ToList(),
            AllTags = allTags
        };

        return View(model);
    }

    /// <summary>
    /// Сохраняет изменения отредактированной статьи
    /// </summary>
    /// <param name="model">Модель с измененными данными статьи</param>
    /// <returns>Перенаправляет на страницу со списком всех статей</returns>
    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(EditArticleViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.AllTags = await _tagRepository.ShowTagsAsync();
            return View(model);
        }

        var article = await _articleRepository.GetArticleByIdAsync(model.Id);
        if (article == null) return NotFound();

        var selectedTags = new List<Tag>();

        if (model.SelectedTagIds != null && model.SelectedTagIds.Any())
        {
            selectedTags = await _tagRepository.GetTagsByIdsAsync(model.SelectedTagIds);
        }

        article.Title = model.Title;
        article.Text = model.Text;
        article.Tags = selectedTags;

        await _articleRepository.SaveAsync();

        _logger.LogInformation(
            "User {UserId} edited article {ArticleId}",
            GetCurrentUserId(),
            article.Id);

        return RedirectToAction(nameof(Index));
    }

    /// <summary>
    /// Удаляет выбранную статью
    /// </summary>
    /// <param name="id">Идентификатор статьи</param>
    /// <returns>Перенаправляет на страницу со списком всех статей</returns>
    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var article = await _articleRepository.GetArticleByIdAsync(id);

        if (article == null)
        {
            return NotFound();
        }
        
        var currentUserId = GetCurrentUserId();
        
        if (article.AuthorId != currentUserId && !User.IsInRole("Admin"))
        {
            return Forbid();
        }
        
        await _articleRepository.DeleteArticleAsync(article);

        _logger.LogInformation(
            "User {UserId} deleted article {ArticleId}",
            currentUserId,
            article.Id);

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