using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizManager.Domain.Constants
{
    public struct RouteNames
    {
        public const string GetHome = "get-home";
        public const string GetLogin = "get-login";
        public const string GetLogout = "get-logout";
        public const string GetUserDashboard = "get-user-dashboard";
        public const string GetAddQuestion = "get-question-add";
        public const string GetCreateQuiz = "get-quiz-create";
        public const string GetDeleteQuiz = "get-quiz-delete";
        public const string GetDeleteQuestion = "get-question-delete";
        public const string GetDeleteAnswer = "get-answer-delete";
        public const string GetViewQuiz = "get-quiz-view";
        public const string GetEditQuiz = "get-quiz-edit";
        public const string GetEditQuestion = "get-question-edit";
        public const string GetViewQuestion = "get-question-view";

        public const string PostLogin = "post-login";
        public const string PostCreateQuiz = "post-quiz-create";
        public const string PostAddQuestion = "post-question-add";
        public const string PostEditQuestion = "post-question-edit";
    }
}
