using System.Security.Claims;
using Blog_final_project.Data;
using Blog_final_project.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog_final_project.Controllers;

public class AccountController : Controller
{
    private readonly BlogDbContext _context;

    public AccountController(BlogDbContext context)
    {
        _context = context;
    }

    // GET
    [HttpGet]
    public IActionResult Login()
    {
        if (User.Identity != null && User.Identity.IsAuthenticated)
        {
            return RedirectToAction("Index", "Articles");
        }

        var model = new LoginViewModel();

        return View(model);
    }

    // POST
    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var user = await _context.Users
            .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Email == model.Login);

        if (user == null)
        {
            ModelState.AddModelError("", "Пользователь не найден");
            return View(model);
        }

        // проверка пароля
        if (user.Password != model.Password)
        {
            ModelState.AddModelError("", "Неверный пароль");
            return View(model);
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName)
        };

        foreach (var role in user.UserRoles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role.Role.Name));
        }

        var claimsIdentity = new ClaimsIdentity(claims, "MyCookieAuth");

        await HttpContext.SignInAsync("MyCookieAuth", new ClaimsPrincipal(claimsIdentity));

        return RedirectToAction("Index", "Home");
    }

    // GET
    public IActionResult Register()
    {
        var model = new RegisterViewModel();

        return View(model);
    }

    // POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        // Проверка существующего email
        var existingUser = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == model.EmailReg);

        if (existingUser != null)
        {
            ModelState.AddModelError("EmailReg", "Пользователь с таким email уже существует");
            return View(model);
        }

        var newUser = new User
        {
            UserName = model.FirstName + " " + model.LastName,
            Email = model.EmailReg,
            Password = model.PasswordReg
        };

        _context.Users.Add(newUser);
        await _context.SaveChangesAsync();

        // Назначаем роль User
        var userRole = await _context.Roles
            .FirstOrDefaultAsync(r => r.Name == "User");

        if (userRole != null)
        {
            var userRoleLink = new UserRole
            {
                UserId = newUser.Id,
                RoleId = userRole.Id
            };

            _context.UserRoles.Add(userRoleLink);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction("Login");
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateUser()
    {
        var roles = await _context.Roles.ToListAsync();

        var model = new CreateUserViewModel
        {
            AllRoles = roles
        };

        return View(model);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateUser(CreateUserViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.AllRoles = await _context.Roles.ToListAsync();
            return View(model);
        }

        var newUser = new User
        {
            UserName = model.FirstName + " " + model.LastName,
            Email = model.Email,
            Password = model.Password
        };

        _context.Users.Add(newUser);
        await _context.SaveChangesAsync();

        // назначаем выбранные роли
        if (model.SelectedRoleIds.Any())
        {
            var userRoles = model.SelectedRoleIds.Select(roleId => new UserRole
            {
                UserId = newUser.Id,
                RoleId = roleId
            });

            await _context.UserRoles.AddRangeAsync(userRoles);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction("Index", "Users");
    }

    [HttpPost]
    [Route("Logout")]
    [ValidateAntiForgeryToken]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync("MyCookieAuth");
        return RedirectToAction("Index", "Home");
    }

    public IActionResult AccessDenied()
    {
        return View();
    }
}
