using QuizManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QuizManager.Domain.Models.QuizViewModels
{
    public class QuizAnswerViewModel
    {
        public QuizAnswerViewModel()
        {
        }

        public QuizAnswerViewModel(Answer answer)
        {
            Id = answer.Id;
            QuestionId = answer?.QuestionId;
            AnswerText = answer?.AnswerText;
            IsCorrectAnswer = answer?.IsCorrectAnswer;
        }
        public long Id { get; set; }
        public long? QuestionId { get; set; }

        [Required(ErrorMessage = "Answer text not provided")]
        public string AnswerText { get; set; }
        public bool? IsCorrectAnswer { get; set; }
    }
}
