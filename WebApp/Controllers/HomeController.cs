using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SOProject.ServiceLayer;
using StackOverflowProject.ViewModels;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        IQuestionService qs;
        ICategoryService cs;

        [ActivatorUtilitiesConstructor]
        public HomeController(IQuestionService qs, ICategoryService cs)
        {
            this.qs = qs;
            this.cs = cs;
        }

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            List<QuestionViewModel> questions = this.qs.GetQuestions().Take(10).ToList();
            return View(questions);
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult Categories()
        {
            List<CategoryViewModel> categories = this.cs.GetCategories();
            return View(categories);
        }

        [Route("allquestions")]
        public IActionResult Questions()
        {
            List<QuestionViewModel> questions = this.qs.GetQuestions();
            return View(questions);
        }

        [HttpPost]
        public IActionResult Search(string str)
        {
            List<QuestionViewModel> questions = this.qs.GetQuestions().Where(temp => temp.QuestionName.ToLower().Contains(str.ToLower()) ||
                temp.Category.CategoryName.ToLower().Contains(str.ToLower())).ToList();
            ViewBag.str = str;
            return View(questions);
        }

    }
}
