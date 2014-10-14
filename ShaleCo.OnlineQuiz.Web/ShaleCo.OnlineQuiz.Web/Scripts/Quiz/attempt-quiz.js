var OnlineQuiz = OnlineQuiz || {};
OnlineQuiz.QuizAttempt = OnlineQuiz.QuizAttempt || function () {
    var bindSubmitButton = function (buttonId) {
        $(buttonId).click(function () {
            var answers = [];
            var count = 1;

            $(".question").each(function () {
                answers.push({
                    QuestionID: $(this).attr("id"),
                    AnswerID: $("input:radio[name='question " + count + "']:checked").val()
                });
                count++;
            });
            var quizAnswers = {
                QuizID: $(".page-header").attr("id"),
                Answers : answers
            };

            for(var i = 0; i < quizAnswers.Answers.length; i++) {
                if (quizAnswers.Answers[i].AnswerID === undefined) {
                    $("#quizAlert").slideDown();
                    return;
                }
            }

            $.ajax({
                url: "/Quiz/Attempt",
                type: "POST",
                data: JSON.stringify(quizAnswers),
                contentType: "application/json",
                success: function (data) {
                    window.location.pathname = "/Quiz/Results/" + quizAnswers.QuizID;
                }
            });
        });
    };

    var initialize = function () {
        bindSubmitButton("#submitButton");

        $("#dismissWarning").click(function () {
            $("#quizAlert").slideUp();
        });
    };

    return {
        Initialize: initialize
    };
}();

$(function () {
    OnlineQuiz.QuizAttempt.Initialize();
});