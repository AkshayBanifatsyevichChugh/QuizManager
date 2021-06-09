using Microsoft.EntityFrameworkCore;
using QuizManager.Domain.Entities;
using QuizManager.Infrastructure;
using QuizManager.Infrastructure.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizManager.Application.Services
{
    public interface IQuizService
    {
        Task AddQuestion(Question question);
        Task CreateQuizAsync(Quiz quiz);
        Task DeleteQuestionAsync(long? id);
        Task DeleteQuizAsync(long? id);
        Task<Question> GetQuestionAsync(long? id, bool shouldTrack = false);
        Task<Quiz> GetQuizAsync(long? id);
        Task<List<Quiz>> GetQuizzesAsync();
        Task UpdateQuestionAsync(Question question);
    }

    public class QuizService : IQuizService
    {
        public readonly QuizContext _context;

        public QuizService(QuizContext context)
        {
            _context = context;
        }

        public async Task AddQuestion(Question question)
        {
            _context.Questions.Add(question);

            await _context.SaveChangesAsync();
        }

        public async Task CreateQuizAsync(Quiz quiz)
        {
            _context.Quizzes.Add(quiz);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteQuestionAsync(long? id)
        {
            var question = new Question { Id = id.Value };

            _context.Remove(question);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteQuizAsync(long? id)
        {
            var quiz = new Quiz { Id = id };

            _context.Quizzes.Remove(quiz);

            await _context.SaveChangesAsync();
        }

        public async Task<Question> GetQuestionAsync(long? id, bool shouldTrack = false)
        {
            var question = await _context.Questions
                .WithQuestionForeignKeys()
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            if (shouldTrack) { _context.Questions.Attach(question); }

            return question;
        }

        public async Task<Quiz> GetQuizAsync(long? id)
        {
            return await _context.Quizzes
                .WithQuizForeignKeys()
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<List<Quiz>> GetQuizzesAsync()
        {
            return await _context.Quizzes
                .WithQuizForeignKeys()
                .ToListAsync();
        }

        public async Task UpdateQuestionAsync(Question question)
        {
            _context.Questions.Update(question);

            await _context.SaveChangesAsync();
        }
    }
}