using Blog_final_project.Data;
using Blog_final_project.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Blog_final_project.Controllers;

public class AccountController : Controller
{
    private readonly BlogDbContext _context;

    public AccountController(BlogDbContext context)
    {
        _context = context;
    }

    // GET
    public IActionResult Login()
    {
        return View();
    }

    // POST
    [HttpPost]
    public async Task<IActionResult> Login(string email, string password)
    {
        var user = await _context.Users
            .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Email == email);

        if (user == null)
            return View();

        // проверка пароля
        if (user.Password != password)
            return View();

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

        await HttpContext.SignInAsync(
            "MyCookieAuth",
            new ClaimsPrincipal(claimsIdentity));

        return RedirectToAction("Index", "Home");
    }

    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync("MyCookieAuth");
        return RedirectToAction("Index", "Home");
    }
}
