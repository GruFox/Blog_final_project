using Blog_final_project.Interfaces;
using Blog_final_project.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CommentsController : ControllerBase
{
    private readonly ICommentRepository _repository;

    public CommentsController(ICommentRepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Получить все комментарии
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var comments = await _repository.GetAllCommentsAsync();
        return Ok(comments);
    }

    /// <summary>
    /// Получить комментарий по id
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var comment = await _repository.GetCommentByIdAsync(id);

        if (comment == null)
            return NotFound();

        return Ok(comment);
    }

    /// <summary>
    /// Создать комментарий
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create(Comment comment)
    {
        await _repository.AddCommentAsync(comment);

        return CreatedAtAction(nameof(Get), new { id = comment.Id }, comment);
    }

    /// <summary>
    /// Обновить комментарий
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Comment comment)
    {
        if (id != comment.Id)
            return BadRequest();

        var existingComment = await _repository.GetCommentByIdAsync(id);

        if (existingComment == null)
            return NotFound();

        await _repository.UpdateCommentAsync(comment);

        return NoContent();
    }

    /// <summary>
    /// Удалить комментарий
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var comment = await _repository.GetCommentByIdAsync(id);

        if (comment == null)
            return NotFound();

        await _repository.DeleteCommentAsync(comment);

        return NoContent();
    }
}
