using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShaleCo.OnlineQuiz.Web.Models
{
    public class QuizViewModel
    {
        public class Quiz
        {
            public string QuizName { get; set; }
            public IList<Question> Questions { get; set; }
        }

        public class Question
        {
            public string Text { get; set; }
            public string CorrectAnswer { get; set; }
            public IList<string> IncorrectAnswers { get; set; }
        }
    }
}