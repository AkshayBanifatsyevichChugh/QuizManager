using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace QuizManager.Domain.Entities
{
    public class Answer
    {
        public Answer(long id, string answerText, bool? isCorrectAnswer)
        {
            Id = id;
            AnswerText = answerText;
            IsCorrectAnswer = isCorrectAnswer;
        }

        public long Id { get; set; }
        [Required]
        public string AnswerText {get; set;}
        [Required]
        public bool? IsCorrectAnswer { get; set; }

        [Required]
        public long? QuestionId { get; set; }

        public Question Question { get; set; }
    }
}
