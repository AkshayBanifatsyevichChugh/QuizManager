using QuizManager.Domain.Entities;
using System.Collections.Generic;

namespace QuizManager.Domain.Models.UserHomeViewModels
{
    public class UserDashboardViewModel
    {
        public UserDashboardViewModel(User user, List<Quiz> quizzes)
        {
            User = user;
            Quizzes = quizzes;
        }

        public User User { get; set; }
        public List<Quiz> Quizzes { get; set; }
    }
}