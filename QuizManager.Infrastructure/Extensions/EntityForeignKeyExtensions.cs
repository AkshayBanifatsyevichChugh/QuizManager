using Microsoft.EntityFrameworkCore;
using QuizManager.Domain.Entities;
using System.Linq;

namespace QuizManager.Infrastructure.Extensions
{
    public static class EntityForeignKeyExtensions
    {
        public static IQueryable<User> WithUserForeignKeys(this IQueryable<User> source)
        {
            return source
                .Include(x => x.UserQuizzes)
                    .ThenInclude(x => x.Questions)
                        .ThenInclude(x => x.Answers);
        }
        public static IQueryable<Quiz> WithQuizForeignKeys(this IQueryable<Quiz> source)
        {
            return source
                .Include(x => x.Questions)
                    .ThenInclude(x => x.Answers);
                
        }
        public static IQueryable<Question> WithQuestionForeignKeys(this IQueryable<Question> source)
        {
            return source
                    .Include(x => x.Answers);
                
        }
    }
}