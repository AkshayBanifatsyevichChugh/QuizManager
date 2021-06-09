using QuizManager.Domain.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QuizManager.Domain.Models.QuizViewModels
{
    public class QuizQuestionViewModel
    {
        public QuizQuestionViewModel()
        {
        }

        public QuizQuestionViewModel(Question question, List<QuizAnswerViewModel> quizAnswerViewModels)
        {
            Id = question.Id;
            QuizId = question?.QuizId;
            QuestionText = question?.QuestionText;
            Answers = quizAnswerViewModels;
        }

        public long Id { get; set; }
        public long? QuizId { get; set; }

        [Required(ErrorMessage = "Question text is cannot be blank.")]
        public string QuestionText { get; set; }

        [Required(ErrorMessage = "Each added question must have 1 answer.")]
        [MinLength(3, ErrorMessage = "A question must have a minimum of 3 answers.")]
        [MaxLength(5, ErrorMessage = "A question must have a maximum of 5 answers.")]
        public List<QuizAnswerViewModel> Answers { get; set; }
    }
}