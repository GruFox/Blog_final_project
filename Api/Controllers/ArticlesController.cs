using Blog_final_project.Interfaces;
using Blog_final_project.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ArticlesController : ControllerBase
{
    private readonly IArticleRepository _repository;

    public ArticlesController(IArticleRepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Получить все статьи
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var articles = await _repository.ShowArticlesAsync();
        return Ok(articles);
    }

    /// <summary>
    /// Получить статью по id
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var article = await _repository.GetArticleByIdAsync(id);

        if (article == null)
            return NotFound();

        return Ok(article);
    }

    /// <summary>
    /// Создать статью
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create(Article article)
    {
        await _repository.CreateArticleAsync(article);

        return CreatedAtAction(nameof(Get), new { id = article.Id }, article);
    }

    /// <summary>
    /// Удалить статью
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var article = await _repository.GetArticleByIdAsync(id);

        if (article == null)
            return NotFound();

        await _repository.DeleteArticleAsync(article);

        return NoContent();
    }
}