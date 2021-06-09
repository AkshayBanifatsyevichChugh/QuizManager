using Microsoft.EntityFrameworkCore;
using QuizManager.Domain.Entities;
using QuizManager.Infrastructure;
using QuizManager.Infrastructure.Extensions;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace QuizManager.Application.Services
{
    public interface IUserService
    {
        Task<User> GetUserAsync(Guid? userIdentifier);
        Task<bool> IsUserTableEmpty();
    }

    public class UserService : IUserService
    {
        private readonly QuizContext _quizContext;

        public UserService(QuizContext quizContext)
        {
            _quizContext = quizContext;
        }

        public async Task<User> GetUserAsync(Guid? userIdentifier)
        {
            return await _quizContext.Users
                .WithUserForeignKeys()
                .Where(x => x.UserIdentifier == userIdentifier)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> IsUserTableEmpty()
        {
            return !await _quizContext.Users.AnyAsync();
        }
    }
}