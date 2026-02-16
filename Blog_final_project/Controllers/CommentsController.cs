using System.Security.Claims;
using Blog_final_project.Data;
using Blog_final_project.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Blog_final_project.Controllers;

[Authorize]
public class CommentsController : Controller
{
    private readonly BlogDbContext _context;

    public CommentsController(BlogDbContext context)
    {
        _context = context;
    }

    // GET: Comments
    public async Task<IActionResult> Index()
    {
        var comments = _context.Comments
            .Include(c => c.Article)
            .Include(c => c.Commentator);

        return View(await comments.ToListAsync());
    }

    // GET: Comments/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var comment = await _context.Comments
            .Include(c => c.Article)
            .Include(c => c.Commentator)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (comment == null) return NotFound();

        return View(comment);
    }

    // GET: Comments/Create
    public IActionResult Create()
    {
        ViewData["ArticleId"] = new SelectList(_context.Articles, "Id", "Title");
        ViewData["CommentatorId"] = new SelectList(_context.Users, "Id", "UserName");
        return View();
    }

    // POST: Comments/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("CommentText,ArticleId")] Comment comment)
    {
        if (ModelState.IsValid)
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            comment.CommentatorId = currentUserId;
            comment.CreatedAt = DateTime.UtcNow;

            _context.Add(comment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        ViewData["ArticleId"] = new SelectList(_context.Articles, "Id", "Title", comment.ArticleId);
        return View(comment);
    }

    // GET: Comments/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
            return NotFound();

        var comment = await _context.Comments
            .Include(c => c.Article)
            .Include(c => c.Commentator)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (comment == null)
            return NotFound();

        var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

        if (comment.CommentatorId != currentUserId
            && !User.IsInRole("Admin"))
        {
            return Forbid();
        }

        return View(comment);
    }

    // POST: Comments/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,CommentText")] Comment updatedComment)
    {
        if (id != updatedComment.Id)
            return NotFound();

        var comment = await _context.Comments.FindAsync(id);

        if (comment == null)
            return NotFound();

        var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

        if (comment.CommentatorId != currentUserId
            && !User.IsInRole("Admin"))
        {
            return Forbid();
        }

        if (ModelState.IsValid)
        {
            comment.CommentText = updatedComment.CommentText;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(comment);
    }

    // GET: Comments/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var comment = await _context.Comments
            .Include(c => c.Article)
            .Include(c => c.Commentator)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (comment == null) return NotFound();

        var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

        if (comment.CommentatorId != currentUserId && !User.IsInRole("Admin"))
        {
            return Forbid();
        }

        return View(comment);
    }

    // POST: Comments/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var comment = await _context.Comments.FindAsync(id);
        if (comment != null)
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            if (comment.CommentatorId != currentUserId && !User.IsInRole("Admin"))
            {
                return Forbid();
            }

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }
}
