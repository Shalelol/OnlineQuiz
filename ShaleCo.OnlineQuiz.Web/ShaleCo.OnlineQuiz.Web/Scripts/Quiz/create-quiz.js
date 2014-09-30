var OnlineQuiz = OnlineQuiz || {};
OnlineQuiz.CreateQuiz = OnlineQuiz.CreateQuiz || function () {

    var bindIncorrectAnswer = function (buttonId) {
        $(buttonId).click(function () {
            var incorrectAnswerGroup = $(this).parent().parent().prev();
            incorrectAnswerGroup = incorrectAnswerGroup.append('<div class="col-lg-2"></div>' +
                '<div class="col-lg-10">' +
                	'<div class="input-group">' +
                		'<input type="text" class="form-control questionIncorrect" placeholder="Incorrect Answer">' +
                		'<span class="input-group-btn">' +
                			'<button class="btn btn-default" type="button">x</button>' +
                            
                		'</span>' +
                	'</div>' +
                '</div>')
            var button = incorrectAnswerGroup.children().children().children().children().last();
            button.click(function () {
                var group = $(this).parent().parent().parent();
                group.prev().remove();
                group.remove();
            });
        });
    };

    var bindAddQuestion = function (buttonId) {
        $(buttonId).click(function () {
            var questionNumber = $(".question").length + 1;
            var insert = $(this).parent().parent().parent().prev();
                insert.before('<form class="form-horizontal">' +
                '<fieldset class="question">' +
                    '<div class="form-group">' +
                        '<label for="question" class="col-lg-2 control-label">Question</label>' +
                        '<div class="col-lg-10">' +
                            '<input type="text" class="form-control questionText" placeholder="Question">' +
                        '</div>' +
                    '</div>' +
                    '<div class="form-group">' +
                        '<label for="questioncorrect" class="col-lg-2 control-label">Correct Answer</label>' +
                        '<div class="col-lg-10">' +
                            '<input type="text" class="form-control questionCorrect" placeholder="Correct Answer">' +
                        '</div>' +
                    '</div>' +
                    '<div class="form-group">' +
                        '<label for="question1incorrect" class="col-lg-2 control-label">Incorrect Answers</label>' +
                        '<div class="col-lg-10">' +
                            '<input type="text" class="form-control questionIncorrect" placeholder="Incorrect Answer">' +
                        '</div>' +
                    '</div>' +
                    '<div class="form-group">' +
                        '<div class="col-lg-12 text-right">' +
                            '<a class="btn btn-default btn-xs" id="addIncorrectAnswer' + questionNumber + '">Add Incorrect Answer</a>' +
                            '<a class="btn btn-default btn-xs" id="deleteQuestion' + questionNumber + '">Delete Question</a>' +
                        '</div>' +
                    '</div>' +
                '</fieldset>' +
            '</form>');
            bindIncorrectAnswer("#addIncorrectAnswer" + questionNumber);
            bindDeleteQuestion("#deleteQuestion" + questionNumber)
        });
    };

    var bindDeleteQuestion = function (buttonId) {
        $(buttonId).click(function () {
            $(this).parent().parent().parent().parent().remove();
        });
    };

    var bindSaveQuiz = function (buttonId) {
        $(buttonId).click(function () {
            var quiz = {
                QuestionID: null,
                AnswerID: null
            };
            quiz.QuizName = $("#quizName").val();
            quiz.Questions = [];
            $(".question").each(function () {
                var $this = $(this);
                var question = {
                    Text: $this.find(".questionText").val(),
                    CorrectAnswer: $this.find(".questionCorrect").val(),
                    IncorrectAnswers: [],
                };
                $this.find(".questionIncorrect").each(function () {
                    question.IncorrectAnswers.push($(this).val());
                });
                quiz.Questions.push(question);
            });
            console.log(quiz);
            //$.post("/Quiz/Create", JSON.stringify(quiz), function () {
            //    console.log(arguments);
            //}, "application/json");
            $.ajax({
                url: "/Quiz/Create",
                type: "POST",
                data: JSON.stringify(quiz),
                contentType: "application/json",
                dataType: "json"
            });
        });
    };

    var initialize = function () {
        bindAddQuestion("#addQuestion");
        bindIncorrectAnswer("#addIncorrectAnswer1");
        bindSaveQuiz("#saveQuiz")
    };

    return {
        Initialize : initialize
    };
}();

$(function () {
    OnlineQuiz.CreateQuiz.Initialize();
});