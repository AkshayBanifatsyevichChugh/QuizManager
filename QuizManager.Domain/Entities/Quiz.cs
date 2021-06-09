using QuizManager.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QuizManager.Domain.Entities
{
    public class Quiz
    {
        public Quiz()
        {
            Questions = new List<Question>();
        }

        public Quiz(string title, List<Question> questions)
        {
            Title = title;

            CreationDate = DateTime.UtcNow;
            QuizIdentifier = Guid.NewGuid();

            Questions = questions ?? new List<Question>();
        }

        [Required]
        public long? Id { get; set; }
        public Guid? QuizIdentifier { get; set; }
        [Required]
        public DateTime? CreationDate { get; set; }
        [Required]
        public string Title { get; set; }

        [Required]
        public long? AuthorId { get; set; }

        public User Author { get; set; }


        public List<Question> Questions { get; set; }       
    }
}