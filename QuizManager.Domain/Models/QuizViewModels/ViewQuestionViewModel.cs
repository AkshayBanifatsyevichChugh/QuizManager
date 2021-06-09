using QuizManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizManager.Domain.Models.QuizViewModels
{
    public class ViewQuestionViewModel
    {
        public ViewQuestionViewModel(Question question)
        {
            Question = question;
        }

        public Question Question { get; set; }
    }
}
