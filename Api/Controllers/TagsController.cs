using Blog_final_project.Interfaces;
using Blog_final_project.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TagsController : ControllerBase
{
    private readonly ITagRepository _repository;

    public TagsController(ITagRepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Получить все теги
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var tags = await _repository.ShowTagsAsync();
        return Ok(tags);
    }

    /// <summary>
    /// Получить тег по id
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var tag = await _repository.GetTagByIdAsync(id);

        if (tag == null)
            return NotFound();

        return Ok(tag);
    }

    /// <summary>
    /// Создать тег
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create(Tag tag)
    {
        if (await _repository.TagExistsAsync(tag.Name))
            return BadRequest("Tag already exists");

        await _repository.CreateTagAsync(tag.Name);

        return Created("", tag);
    }
}
