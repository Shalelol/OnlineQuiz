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
            var attempt = _context.QuizAttempts.FirstOrDefault(e => e.StudentName == User.Identity.Name);

            if(attempt != null)
            {
                return RedirectToAction("Results", new { id = id });
            }

            var quiz = _context.Quizzes.First(e => e.QuizID == id);

            return View(quiz);
        }

        [HttpPost]
        public ActionResult Attempt(QuizViewModel.QuizAnswers data)
        {
            var quiz = _context.Quizzes.First(e => e.QuizID == data.QuizID);
            var quizAttempt = new QuizAttempt();
            quizAttempt.QuizID = quiz.QuizID;
            quizAttempt.StudentName = User.Identity.Name;
            
            foreach(var answer in data.Answers)
            {
                var quizAnswer = new QuizAnswer();
                quizAnswer.QuestionID = answer.QuestionID;
                quizAnswer.AnswerID = answer.AnswerID;

                quizAttempt.Answers.Add(quizAnswer);
            }

            _context.QuizAttempts.Add(quizAttempt);
            _context.SaveChanges();

            return null;
        }

        public ActionResult Results(int id)
        {
            var quizAttempt = _context.QuizAttempts.FirstOrDefault(e => e.QuizID == id && e.StudentName == User.Identity.Name);

            var resultsViewModel = new QuizViewModel.QuizResults();
            resultsViewModel.Score = this.Score(quizAttempt);
            resultsViewModel.QuizAttempt = quizAttempt;

            return View(resultsViewModel);
        }

        public ActionResult Details(int id)
        {
            var model = new QuizViewModel.QuizDetails();
            model.Quiz = this.MapQuiz(_context.Quizzes.FirstOrDefault(e => e.QuizID == id));

            var quizAttempts = _context.QuizAttempts.Where(e => e.QuizID == id).ToList();

            var total = 0;
            foreach(var attempt in quizAttempts)
            {
                var score = this.Score(attempt);
                model.Scores.Add(new QuizViewModel.QuizScore(attempt.StudentName, score));
                total += score;
            }

            model.Average = total / (double) model.Scores.Count;

            return View(model);
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

            return null;
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
            quiz.QuizID = data.QuizID;
            
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

        private QuizViewModel.QuizAnswers MapQuizAttempt(QuizAttempt data)
        {
            var quizAnswers = new QuizViewModel.QuizAnswers();
            quizAnswers.QuizID = data.QuizID;
            
            foreach(var answer in data.Answers)
            {
                var questionAnswer = new QuizViewModel.QuestionAnswer();
                questionAnswer.AnswerID = (int) answer.AnswerID;
                questionAnswer.QuestionID = (int) answer.QuestionID;
                quizAnswers.Answers.Add(questionAnswer);
            }

            return quizAnswers;
        }

        private int Score(QuizAttempt quizAttempt)
        {
            var correct = 0;

            foreach(var answer in quizAttempt.Answers)
            {
                if(quizAttempt.Quiz.Questions.First(e => e.QuestionID == answer.QuestionID).CorrectAnswer.AnswerID == answer.AnswerID)
                {
                    correct++;
                }
            }

            return correct;
        }
	}
}