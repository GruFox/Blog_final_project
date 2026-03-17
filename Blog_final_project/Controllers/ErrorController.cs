using Microsoft.AspNetCore.Mvc;

namespace Blog_final_project.Controllers
{
    public class ErrorController : Controller
    {
        // Forbid() - 403
        /// <summary>
        /// Выводит причину запрета
        /// </summary>
        /// <returns>Представление с причиной запрета</returns>
        [HttpGet]
        [Route("Error/AccessDenied")]
        public IActionResult AccessDenied()
        {
            return View();
        }

        // NotFound() - 404
        /// <summary>
        /// Выводит причину неотображения страницы
        /// </summary>
        /// <returns>Представление с причиной неотображения страницы</returns>
        [HttpGet]
        [Route("Error/NotFound")]
        public IActionResult NotFoundPage()
        {
            return View("NotFound");
        }

        // Exception - 500
        /// <summary>
        /// Выводит ошибку сервера
        /// </summary>
        /// <returns>Представление о внутренней ошибке сервера</returns>
        [HttpGet]
        [Route("Error/ServerError")]
        public IActionResult ServerError()
        {
            return View();
        }
    }
}
