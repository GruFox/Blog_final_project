using Blog_final_project.Interfaces;
using Blog_final_project.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserRepository _repository;

    public UsersController(IUserRepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Получить всех пользователей
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var users = await _repository.ShowUsersAsync();
        return Ok(users);
    }

    /// <summary>
    /// Получить пользователя по id
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var user = await _repository.GetUserByIdAsync(id);

        if (user == null)
            return NotFound();

        return Ok(user);
    }
}
