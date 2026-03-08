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

    public ArticlesController(IArticleRepository articleRepository, ITagRepository tagRepository)
    {
        _articleRepository = articleRepository;
        _tagRepository = tagRepository;
    }

    // GET: Articles
    public async Task<IActionResult> Index()
    {
        var articles = await _articleRepository.ShowArticlesAsync();

        var model = new ArticleViewModel
        {
            Articles = articles
        };

        return View(model);
    }

    // GET: Articles/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var article = await _articleRepository.GetArticleById(id);

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

    // GET: Articles/Create
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

    // POST: Articles/Create
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

        return RedirectToAction(nameof(Index));
    }

    // GET: Articles/Edit/5
    [Authorize]
    public async Task<IActionResult> Edit(int id)
    {
        var article = await _articleRepository.GetArticleById(id);

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

    // POST: Articles/Edit/5
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

        var article = await _articleRepository.GetArticleById(model.Id);
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

        return RedirectToAction(nameof(Index));
    }

    // POST: Articles/Delete/5
    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var article = await _articleRepository.GetArticleById(id);

        if (article == null)
        {
            return NotFound();
        }
        
        var currentUserId = GetCurrentUserId();
        
        if (article.AuthorId != currentUserId && !User.IsInRole("Admin"))
        {
            return Forbid();
        }
        
        await _articleRepository.DeleteAsync(article);

        return RedirectToAction(nameof(Index));
    }

    private int GetCurrentUserId()
    {
        var claim = User.FindFirst(ClaimTypes.NameIdentifier);

        if (claim == null)
            throw new Exception("User id claim not found");

        return int.Parse(claim.Value);
    }
}