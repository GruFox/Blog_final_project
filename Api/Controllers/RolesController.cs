using Blog_final_project.Interfaces;
using Blog_final_project.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RolesController : ControllerBase
{
    private readonly IRoleRepository _repository;

    public RolesController(IRoleRepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Получить все роли
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var roles = await _repository.GetAllRolesAsync();
        return Ok(roles);
    }

    /// <summary>
    /// Получить роль по id
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var role = await _repository.GetRoleById(id);

        if (role == null)
            return NotFound();

        return Ok(role);
    }

    /// <summary>
    /// Создать роль
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create(Role role)
    {
        if (await _repository.RoleExistsAsync(role.Name))
            return BadRequest("Role already exists");

        await _repository.CreateRoleAsync(role.Name, role.Description);

        return Created("", role);
    }
}
