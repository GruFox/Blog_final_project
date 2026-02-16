using Blog_final_project.Data;
using Blog_final_project.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Blog_final_project.Controllers;

public class ArticlesController : Controller
{
    public readonly BlogDbContext _context;

    public ArticlesController(BlogDbContext context)
    {
        _context = context;
    }

    // GET: Articles
    [Authorize]
    public async Task<IActionResult> Index()
    {
        var articles = await _context.Articles
            .Include(a => a.Author)
            .ToListAsync();
        return View(articles);
    }

    // GET: Articles/Details/5
    [Authorize]
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var article = await _context.Articles
            .Include(a => a.Author)
            .Include(a => a.Comments)
            .Include(a => a.Tags)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (article == null) return NotFound();

        return View(article);
    }

    // GET: Articles/Create
    [Authorize]
    public IActionResult Create()
    {
        ViewData["Tags"] = _context.Tags.ToList();
        return View();
    }

    // POST: Articles/Create
    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Title,Text")] Article article, int[] selectedTags)
    {
        if (ModelState.IsValid)
        {
            var currentUserId = GetCurrentUserId();

            article.AuthorId = currentUserId;

            // Добавляем теги
            if (selectedTags != null)
            {
                foreach (var tagId in selectedTags)
                {
                    var tag = await _context.Tags.FindAsync(tagId);
                    if (tag != null) article.Tags.Add(tag);
                }
            }

            _context.Add(article);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        ViewData["Tags"] = _context.Tags.ToList();
        return View(article);
    }

    // GET: Articles/Edit/5
    [Authorize]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var article = await _context.Articles
            .Include(a => a.Tags)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (article == null) return NotFound();

        var currentUserId = GetCurrentUserId();

        if (article.AuthorId != currentUserId
            && !User.IsInRole("Admin")
            && !User.IsInRole("Moderator"))
        {
            return Forbid();
        }

        ViewData["Tags"] = _context.Tags.ToList();
        return View(article);
    }

    // POST: Articles/Edit/5
    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Text")] Article updatedArticle, int[] selectedTags)
    {
        if (id != updatedArticle.Id)
            return NotFound();

        var article = await _context.Articles
            .Include(a => a.Tags)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (article == null)
            return NotFound();

        var currentUserId = GetCurrentUserId();

        if (article.AuthorId != currentUserId
            && !User.IsInRole("Admin")
            && !User.IsInRole("Moderator"))
            return Forbid();

        if (ModelState.IsValid)
        {
            article.Title = updatedArticle.Title;
            article.Text = updatedArticle.Text;

            article.Tags.Clear();
            foreach (var tagId in selectedTags)
            {
                var tag = await _context.Tags.FindAsync(tagId);
                if (tag != null) article.Tags.Add(tag);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        ViewData["Tags"] = _context.Tags.ToList();
        return View(article);
    }


    // GET: Articles/Delete/5
    [Authorize]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var article = await _context.Articles
            .Include(a => a.Author)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (article == null) return NotFound();

        var currentUserId = GetCurrentUserId();

        if (article.AuthorId != currentUserId && !User.IsInRole("Admin"))
        {
            return Forbid();
        }

        return View(article);
    }

    // POST: Articles/Delete/5
    [Authorize]
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var article = await _context.Articles.FindAsync(id);
        if (article != null)
        {
            var currentUserId = GetCurrentUserId();

            if (article.AuthorId != currentUserId && !User.IsInRole("Admin"))
            {
                return Forbid();
            }

            _context.Articles.Remove(article);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    private int GetCurrentUserId()
    {
        return int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
    }
}