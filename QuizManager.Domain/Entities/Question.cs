using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QuizManager.Domain.Entities
{
    public class Question
    {
        public Question()
        {
            Answers = new List<Answer>();
        }

        public Question(long id, long? quizId, string questionText, List<Answer> answers)
        {
            Id = id;
            QuizId = quizId;
            QuestionText = questionText;
            Answers = answers;
        }

        public long Id { get; set; }
        public string QuestionText { get; set; }

        [Required]
        public long? QuizId { get; set; }

        public Quiz Quiz { get; set; }
        public List<Answer> Answers { get; set; }
        
    }
}
