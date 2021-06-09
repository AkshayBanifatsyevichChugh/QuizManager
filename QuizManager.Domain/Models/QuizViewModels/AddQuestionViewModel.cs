using QuizManager.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace QuizManager.Domain.Models.QuizViewModels
{
    public class AddQuestionViewModel
    {
        public AddQuestionViewModel()
        {
        }

        public AddQuestionViewModel(Quiz quiz, QuizQuestionViewModel question)
        {
            Quiz = quiz;
            Question = question;
        }

        public Quiz Quiz { get; set; }

        [Required(ErrorMessage = "Question details cannot be blank.")]
        public QuizQuestionViewModel Question { get; set; }
    }
}