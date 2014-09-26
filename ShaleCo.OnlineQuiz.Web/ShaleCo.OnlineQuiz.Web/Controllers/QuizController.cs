using ShaleCo.OnlineQuiz.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShaleCo.OnlineQuiz.Web.Controllers
{
    public class QuizController : Controller
    {
        private QuizDbContext _context = new QuizDbContext();

        //
        // GET: /Quiz/
        public ActionResult Index()
        {
            if (false)
            {
                var quiz = new Quiz();
                quiz.TeacherName = User.Identity.Name;

                var question = new Question();
                question.Text = "What is 2 + 2?";

                var answer1 = new Answer();
                answer1.Text = "5";

                var answer2 = new Answer();
                answer2.Text = "4";

                var answer3 = new Answer();
                answer3.Text = "1,000,000";

                var answer4 = new Answer();
                answer4.Text = "15";

                question.Answers.Add(answer1);
                question.Answers.Add(answer3);
                question.Answers.Add(answer4);
                question.CorrectAnswer = answer2;

                quiz.Questions.Add(question);

                _context.Quizzes.Add(quiz);

                _context.SaveChanges();
            }

            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Quiz quiz)
        {
            return View();
        }
	}
}