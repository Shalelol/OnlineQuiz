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

            $.ajax({
                url: "/Quiz/Attempt",
                type: "POST",
                data: JSON.stringify(quizAnswers),
                contentType: "application/json",
                dataType: "json"
            });
        });
    };

    var initialize = function () {
        bindSubmitButton("#submitButton");
    };

    return {
        Initialize: initialize
    };
}();

$(function () {
    OnlineQuiz.QuizAttempt.Initialize();
});