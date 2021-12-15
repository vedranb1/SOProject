using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SOProject.ServiceLayer;
using StackOverflowProject.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Controllers
{
    public class QuestionsController : Controller
    {
        IQuestionService qs;
        IAnswerService asr;
        ICategoryService cs;
        IUserService us;

        public QuestionsController(IQuestionService qs, IAnswerService asr, ICategoryService cs, IUserService us)
        {
            this.qs = qs;
            this.asr = asr;
            this.cs = cs;
            this.us = us;
        }

        public IActionResult Create()
        {
            List<CategoryViewModel> categories = this.cs.GetCategories();
            ViewBag.categories = categories;
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        
        public IActionResult Create(NewQuestionViewModel nqvm)
        {
            if (ModelState.IsValid)
            {
                nqvm.AnswerCount = 0;
                nqvm.QuestionDateAndTime = DateTime.Now;
                nqvm.ViewsCount = 0;
                nqvm.VotesCount = 0;
                nqvm.UserID = Convert.ToInt32(HttpContext.Session.GetInt32("CurrentUserID"));
                this.qs.InsertQuestion(nqvm);
                return RedirectToAction("Questions", "Home");
            }
            else
            {
                ModelState.AddModelError("x", "Invalid data.");
                return View();
            }
        }

        public IActionResult View(int id)
        {
            this.qs.UpdateQuestionViewsCount(id, 1);
            int uid = Convert.ToInt32(HttpContext.Session.GetInt32("CurrentUserID"));
            QuestionViewModel qvm = this.qs.GetQuestionsByQuestionID(id, uid);
            return View(qvm);
        }

        

        
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult EditAnswer(EditAnswerViewModel eavm)
        {
            if (ModelState.IsValid)
            {
                eavm.UserID = Convert.ToInt32(HttpContext.Session.GetInt32("CurrentUserID"));
                asr.UpdateAnswerDetails(eavm);
                return RedirectToAction("View", new { id = eavm.UserID });
            }
            else
            {
                ModelState.AddModelError("x", "Invalid data.");
                return RedirectToAction("View", new { id = eavm.UserID });
            }
        }

        
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult AddAnswer(NewAnswerViewModel navm)
        {
            foreach (var key in HttpContext.Session.Keys)
            {
                Console.WriteLine(key + HttpContext.Session.GetString(key));
            }

            navm.UserID = Convert.ToInt32(HttpContext.Session.GetInt32("CurrentUserID"));
            navm.AnswerDateAndTime = DateTime.Now;
            navm.VotesCount = 0;
            int uid = Convert.ToInt32(HttpContext.Session.GetInt32("CurrentUserID"));
            QuestionViewModel qvm = this.qs.GetQuestionsByQuestionID(navm.QuestionID, uid);
            if (ModelState.IsValid)
            {
                asr.InsertAnswer(navm);
            }
            else
            {
                ModelState.AddModelError("x", "Invalid data.");
            }
            return View("View", qvm);
        }
    }
}
