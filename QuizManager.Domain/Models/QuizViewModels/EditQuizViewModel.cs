using QuizManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizManager.Domain.Models.QuizViewModels
{
    public class EditQuizViewModel
    {
        public EditQuizViewModel(Quiz quiz)
        {
            Quiz = quiz;
        }

        public Quiz Quiz { get; set; }
    }
}
