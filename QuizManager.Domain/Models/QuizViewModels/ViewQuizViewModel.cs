using QuizManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizManager.Domain.Models.QuizViewModels
{
    public class ViewQuizViewModel
    {
        public ViewQuizViewModel(User user, Quiz quiz)
        {
            User = user;
            Quiz = quiz;
        }

        public User User { get; set; }
        public Quiz Quiz { get; set; }
    }
}
