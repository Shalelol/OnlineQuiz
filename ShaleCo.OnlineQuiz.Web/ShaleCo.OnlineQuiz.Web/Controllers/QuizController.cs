using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using ShaleCo.OnlineQuiz.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShaleCo.OnlineQuiz.Web.Controllers
{
    [Authorize]
    public class QuizController : Controller
    {
        private QuizDbContext _context = new QuizDbContext();
        private UserManager<ApplicationUser> _userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));

        public ActionResult Index()
        {
            string teacherName = "";
            if (User.IsInRole(UserRoles.Teacher.ToString()))
            {
                teacherName = User.Identity.Name;
            }
            else if (User.IsInRole(UserRoles.Student.ToString()))
            {
                teacherName = _userManager.FindByName(User.Identity.Name).Teacher;
            }

            var quizzes = _context.Quizzes.Where(e => e.TeacherName == teacherName).ToList();

            var model = new QuizViewModel.ListViewModel();

            foreach(var quiz in quizzes)
            {
                model.Quizzes.Add(this.MapQuiz(quiz));
            }

            return View(model);
        }

        public ActionResult Attempt(int id)
        {
            var quiz = _context.Quizzes.First(e => e.QuizID == id);

            return View(quiz);
        }

        [HttpPost]
        public ActionResult Attempt(QuizViewModel.QuizAnswers data)
        {
            throw new NotImplementedException();
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(QuizViewModel.Quiz data)
        {
            var quiz = this.MapQuiz(data);
            _context.Quizzes.Add(quiz);
            _context.SaveChanges();

            return View();
        }

        private Quiz MapQuiz(QuizViewModel.Quiz data)
        {
            var quiz = new Quiz();
            quiz.QuizName = data.QuizName;
            quiz.TeacherName = User.Identity.Name;

            foreach(var questionData in data.Questions)
            {
                var question = new Question();
                question.Text = questionData.Text;
                question.CorrectAnswer = new Answer() { Text = questionData.CorrectAnswer };

                foreach(var incorrectData in questionData.IncorrectAnswers)
                {
                    question.Answers.Add(new Answer() { Text = incorrectData });
                }

                quiz.Questions.Add(question);
            }

            return quiz;
        }

        private QuizViewModel.Quiz MapQuiz(Quiz data)
        {
            var quiz = new QuizViewModel.Quiz();
            quiz.QuizName = data.QuizName;
            
            foreach(var questionData in data.Questions)
            {
                var question = new QuizViewModel.Question();
                question.Text = questionData.Text;
                question.CorrectAnswer = questionData.CorrectAnswer.Text;

                foreach(var incorrectData in questionData.Answers)
                {
                    question.IncorrectAnswers.Add(incorrectData.Text);
                }

                quiz.Questions.Add(question);
            }

            return quiz;
        }
	}
}