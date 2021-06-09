using QuizManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizManager.Domain.Models.QuizViewModels
{
    public class EditQuestionViewModel
    {
        public EditQuestionViewModel()
        {
        }

        public EditQuestionViewModel(Quiz quiz, QuizQuestionViewModel question)
        {
            Quiz = quiz;
            Question = question;
        }

        public Quiz Quiz { get; set; }
        [Required(ErrorMessage = "Question details cannot be blank.")]
        public QuizQuestionViewModel Question {get; set;}
    }
}
