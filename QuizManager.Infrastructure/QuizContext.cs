using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using QuizManager.Domain.Entities;
using QuizManager.Infrastructure.Extensions;

namespace QuizManager.Infrastructure
{
    public class QuizContext : IdentityDbContext<User, IdentityRole<long>, long>
    {
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IConfiguration _configuration;
        public QuizContext(IPasswordHasher<User> passwordHasher, IConfiguration configuration)
        {
            _passwordHasher = passwordHasher;
            _configuration = configuration;
            
            Database.EnsureCreated();
        }

        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answer { get; set; }
        public override DbSet<User> Users { get; set; }
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder
                .SetUpUserForeignKeys()
                .SetUpQuizForeignKeys()
                .SetUpQuestionForeignKeys()
                .SetUpAnswerForeignKeys()
                .SetUpTestUsers(_passwordHasher);

            base.OnModelCreating(builder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_configuration["SqlServerConnectionString"]);
            optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

            base.OnConfiguring(optionsBuilder);
        }
    }
}