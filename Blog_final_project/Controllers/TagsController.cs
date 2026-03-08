using System.Data;
using Blog_final_project.Interfaces;
using Blog_final_project.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog_final_project.Controllers;

[Authorize]
public class TagsController : Controller
{

    private readonly ITagRepository _tagRepository;

    public TagsController(ITagRepository tagRepository)
    {

        _tagRepository = tagRepository;
    }

    // GET: Tags
    public async Task<IActionResult> Index()
    {
        var tags = await _tagRepository.ShowTagsAsync();

        var model = new TagViewModel
        {
            Tags = tags
        };

        return View(model);
    }

    // GET: Tags/Create
    [Authorize(Roles = "Admin,Moderator")]
    public IActionResult Create()
    {
        var model = new CreateTagViewModel();

        return View(model);
    }

    // POST: Tags/Create
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

        return RedirectToAction(nameof(Index));
    }

    // GET: Tags/Edit/5
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

    // POST: Tags/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(EditTagViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        if (await _tagRepository.TagExistsAsync(model.Name))
        {
            ModelState.AddModelError("Name", "Такой тег уже существует");
            return View(model);
        }

        var tag = await _tagRepository.GetTagByIdAsync(model.Id);
        if (tag == null) return NotFound();

        tag.Name = model.Name;

        await _tagRepository.SaveAsync();

        return RedirectToAction(nameof(Index));
    }
}
