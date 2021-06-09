using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QuizManager.Domain.Models.QuizViewModels
{
    public class CreateQuizViewModel
    {
        [Required(ErrorMessage = "Quiz title is required")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Quiz must have at least one question")]
        public List<QuizQuestionViewModel> Questions { get; set; }
    }
}
