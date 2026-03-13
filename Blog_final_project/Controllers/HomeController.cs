using System.Diagnostics;
using Blog_final_project.Interfaces;
using Blog_final_project.Models;
using Microsoft.AspNetCore.Mvc;

namespace Blog_final_project.Controllers
{
    public class HomeController : Controller
    {
        private readonly IArticleRepository _articleRepository;

        public HomeController(IArticleRepository articleRepository)
        {
            _articleRepository = articleRepository;
        }

        /// <summary>
        /// Отображает список всех статей
        /// </summary>
        /// <returns>Представление со списком всех статей</returns>
        public async Task<IActionResult> Index()
        {
            var articles = await _articleRepository.ShowArticlesAsync();

            var model = new ArticleViewModel { Articles = articles };

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
