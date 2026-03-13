using System.Security.Claims;
using Blog_final_project.Interfaces;
using Blog_final_project.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog_final_project.Controllers;

[Authorize]
public class CommentsController : Controller
{
    private readonly ICommentRepository _commentRepository;
    private readonly ILogger<CommentsController> _logger;

    public CommentsController(ICommentRepository commentRepository, ILogger<CommentsController> logger)
    {
        _commentRepository = commentRepository;
        _logger = logger;
    }

    /// <summary>
    /// Отображает список всех комментариев
    /// </summary>
    /// <returns>Представление со списком всех комментариев</returns>
    public async Task<IActionResult> Index()
    {
        var comments = await _commentRepository.GetAllCommentsAsync();

        var model = new CommentViewModel
        {
            Comments = comments
        };

        return View(model);
    }

    /// <summary>
    /// Отображает подробные данные выбранного комментария
    /// </summary>
    /// <param name="id">Идентификатор комментария</param>
    /// <returns>Представление с данными выбранного комментария</returns>
    public async Task<IActionResult> Details(int id)
    {
        var comment = await _commentRepository.GetCommentByIdAsync(id);

        if (comment == null)
            return NotFound();

        return View(comment);
    }

    /// <summary>
    /// Сохраняет созданный комментарий
    /// </summary>
    /// <param name="model">Модель с данными комментария</param>
    /// <returns>Перенаправляет на страницу статьи с этим комментарием</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateCommentViewModel model)
    {
        if (!ModelState.IsValid)
            return RedirectToAction("Details", "Articles", new { id = model.ArticleId });

        var userId = GetCurrentUserId();

        var comment = new Comment
        {
            CommentText = model.CommentText,
            ArticleId = model.ArticleId,
            CommentatorId = userId,
            CreatedAt = DateTime.UtcNow
        };

        await _commentRepository.AddCommentAsync(comment);

        _logger.LogInformation(
            "User {UserId} added comment to article {ArticleId}",
            userId,
            model.ArticleId);

        return RedirectToAction("Details", "Articles", new { id = model.ArticleId });
    }

    /// <summary>
    /// Отображает форму редактирования выбранного комментария
    /// </summary>
    /// <param name="id">Идентификатор комментария</param>
    /// <returns>Представление с формой редактирования выбранного комментария</returns>
    public async Task<IActionResult> Edit(int id)
    {
        var comment = await _commentRepository.GetCommentByIdAsync(id);

        if (comment == null)
            return NotFound();

        if (comment.CommentatorId != GetCurrentUserId() && !User.IsInRole("Admin"))
            return Forbid();

        var model = new EditCommentViewModel
        {
            Id = comment.Id,
            CommentText = comment.CommentText
        };

        return View(model);
    }

    /// <summary>
    /// Сохраняет измененный комментарий
    /// </summary>
    /// <param name="model">Модель с измененными данными</param>
    /// <returns>Перенаправляет на страницу статьи с этим комментарием</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(EditCommentViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var comment = await _commentRepository.GetCommentByIdAsync(model.Id);

        if (comment == null)
            return NotFound();

        if (comment.CommentatorId != GetCurrentUserId() && !User.IsInRole("Admin"))
            return Forbid();

        comment.CommentText = model.CommentText;

        await _commentRepository.UpdateCommentAsync(comment);

        _logger.LogInformation(
            "User {UserId} edited comment {CommentId}",
            GetCurrentUserId(),
            comment.Id);

        return RedirectToAction("Details", "Articles", new { id = comment.ArticleId });
    }

    /// <summary>
    /// Удаляет выбранный комментарий
    /// </summary>
    /// <param name="id">Идентификатор комментария</param>
    /// <returns>Перенаправляет на страницу статьи с этим комментарием</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var comment = await _commentRepository.GetCommentByIdAsync(id);

        if (comment == null)
            return NotFound();

        if (comment.CommentatorId != GetCurrentUserId() && !User.IsInRole("Admin"))
            return Forbid();

        var articleId = comment.ArticleId;

        await _commentRepository.DeleteCommentAsync(comment);

        _logger.LogInformation(
            "User {UserId} deleted comment {CommentId}",
            GetCurrentUserId(),
            id);

        return RedirectToAction("Details", "Articles", new { id = articleId });
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