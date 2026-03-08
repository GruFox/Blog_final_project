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

    public CommentsController(ICommentRepository commentRepository)
    {
        _commentRepository = commentRepository;
    }

    public async Task<IActionResult> Index()
    {
        var comments = await _commentRepository.GetAllCommentsAsync();

        var model = new CommentViewModel
        {
            Comments = comments
        };

        return View(model);
    }

    public async Task<IActionResult> Details(int id)
    {
        var comment = await _commentRepository.GetCommentByIdAsync(id);

        if (comment == null)
            return NotFound();

        return View(comment);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateCommentViewModel model)
    {
        if (!ModelState.IsValid)
            return RedirectToAction("Details", "Articles", new { id = model.ArticleId });

        var comment = new Comment
        {
            CommentText = model.CommentText,
            ArticleId = model.ArticleId,
            CommentatorId = GetCurrentUserId(),
            CreatedAt = DateTime.UtcNow
        };

        await _commentRepository.AddCommentAsync(comment);

        return RedirectToAction("Details", "Articles", new { id = model.ArticleId });
    }

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

        return RedirectToAction("Details", "Articles", new { id = comment.ArticleId });
    }

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

        return RedirectToAction("Details", "Articles", new { id = articleId });
    }

    private int GetCurrentUserId()
    {
        var claim = User.FindFirst(ClaimTypes.NameIdentifier);

        if (claim == null)
            throw new Exception("User id claim not found");

        return int.Parse(claim.Value);
    }
}