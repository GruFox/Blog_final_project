using Blog_final_project.Interfaces;
using Blog_final_project.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog_final_project.Controllers;

[Authorize]
public class RolesController : Controller
{
    private IRoleRepository _roleRepository;

    public RolesController(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    // GET: Roles
    public async Task<IActionResult> Index()
    {
        var roles = await _roleRepository.ShowRolesAsync();
        
        var model = new RoleViewModel()
        {
            Roles = roles
        };

        return View(model);
    }

    // GET: Roles/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var role = await _roleRepository.GetRoleById(id);

        if (role == null)
            return NotFound();

        var model = new DetailsRoleViewModel
        {
            Id = role.Id,
            Name = role.Name,
            Description = role.Description
        };

        return View(model);
    }

    // GET: Roles/Create
    [Authorize(Roles = "Admin")]
    public IActionResult Create()
    {
        var model = new CreateRoleViewModel();

        return View(model);
    }

    // POST: Roles/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateRoleViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        if (await _roleRepository.RoleExistsAsync(model.Name))
        {
            ModelState.AddModelError("Name", "Такая роль уже существует");
            return View(model);
        }

        await _roleRepository.CreateRoleAsync(model.Name, model.Description);

        return RedirectToAction(nameof(Index));
    }

    // GET: Roles/Edit/5
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(int id)
    {
        var role = await _roleRepository.GetRoleById(id);

        if (role == null) return NotFound();

        var model = new EditRoleViewModel
        {
            Id = role.Id,
            Name = role.Name,
            Description = role.Description
        };

        return View(model);
    }

    // POST: Roles/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(EditRoleViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var role = await _roleRepository.GetRoleById(model.Id);
        if (role == null) return NotFound();

        role.Name = model.Name;
        role.Description = model.Description;

        await _roleRepository.SaveAsync();

        return RedirectToAction(nameof(Index));
    }
}
