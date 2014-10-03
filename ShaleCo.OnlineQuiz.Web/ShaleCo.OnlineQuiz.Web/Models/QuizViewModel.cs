using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShaleCo.OnlineQuiz.Web.Models
{
    public class QuizViewModel
    {
        public class ListViewModel
        {
            public ListViewModel()
            {
                this.Quizzes = new List<Quiz>();
            }

            public IList<Quiz> Quizzes { get; set; }
        }

        public class Quiz
        {
            public Quiz()
            {
                this.Questions = new List<Question>();
            }

            public int QuizID { get; set; }
            public string QuizName { get; set; }
            public IList<Question> Questions { get; set; }
        }

        public class Question
        {
            public Question()
            {
                this.IncorrectAnswers = new List<string>();
            }

            public string Text { get; set; }
            public string CorrectAnswer { get; set; }
            public IList<string> IncorrectAnswers { get; set; }
        }

        public class QuizAnswers
        {
            public QuizAnswers()
            {
                this.Answers = new List<QuestionAnswer>();
            }

            public int QuizID { get; set; }
            public IList<QuestionAnswer> Answers { get; set; }
        }

        public class QuestionAnswer
        {
            public int QuestionID { get; set; }
            public int AnswerID { get; set; }
        }

        public class QuizResults
        {
            public int Score { get; set; }
            public Quiz Quiz { get; set; }
            public QuizAnswers QuizAttempt { get; set; }
        }
    }
}