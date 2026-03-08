using Microsoft.AspNetCore.Mvc;

namespace Blog_final_project.Controllers
{
    public class ErrorController : Controller
    {
        // Forbid() - 403
        [Route("Error/AccessDenied")]
        public IActionResult AccessDenied()
        {
            return View();
        }

        // NotFound() - 404
        [Route("Error/NorFound")]
        public IActionResult NotFoundPage()
        {
            return View("NotFound");
        }

        // Exception - 500
        [Route("Error/ServerError")]
        public IActionResult ServerError()
        {
            return View();
        }
    }
}
