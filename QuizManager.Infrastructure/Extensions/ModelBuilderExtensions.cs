using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QuizManager.Domain.Constants;
using QuizManager.Domain.Entities;
using QuizManager.Domain.Enums;
using System;
using System.Collections.Generic;

namespace QuizManager.Infrastructure.Extensions
{
    public static class ModelBuilderExtensions
    {
        //private async Task ConfigureDefaultUsers(UserManager<User> userManager, IUserService userService)
        //{
        //    var isTableEmpty = await userService.IsUserTableEmpty();

        //    if (!isTableEmpty) { return; }

        //    await Task.WhenAll(defaultUsers.Select(x => userManager.CreateAsync(x, "TestUser1")));
        //}

        //private async Task AddClaims(User user, UserManager<User> userManager)
        //{
        //    var claim = new Claim(ClaimNames.UserIdentifier, user.UserIdentifier.ToString());

        //    await userManager.AddClaimAsync(user, claim);
        //}

        public static ModelBuilder SetUpTestUsers(this ModelBuilder builder, IPasswordHasher<User> passwordHasher)
        {
            var defaultUsers = new List<User>();
            var claims = new List<IdentityUserClaim<long>>();

            defaultUsers.Add(new User("restricted@user.com", PermissionLevel.Restricted, null) { Id = 1});
            defaultUsers.Add(new User("view@user.com", PermissionLevel.View, null) { Id = 2 });
            defaultUsers.Add(new User("edit@user.com", PermissionLevel.Edit, null) { Id = 3 });

            defaultUsers.ForEach(x =>
            {
                x.SecurityStamp = Guid.NewGuid().ToString();
                x.NormalizedUserName = x.Email;
                x.NormalizedEmail = x.Email;
                x.PasswordHash = passwordHasher.HashPassword(x, "TestUser1");

                var claim = new IdentityUserClaim<long>();

                claim.ClaimType = ClaimNames.UserIdentifier;
                claim.ClaimValue = x.UserIdentifier.ToString();
                claim.Id = Convert.ToInt32(x.Id);
                claim.UserId = x.Id;

                claims.Add(claim);
            });

            builder
                .Entity<User>()
                .HasData(defaultUsers);

            builder
                .Entity<IdentityUserClaim<long>>()
                .HasData(claims);

            return builder;
        }

        public static ModelBuilder SetUpUserForeignKeys(this ModelBuilder builder)
        {
            builder
                .Entity<User>()
                .HasMany(x => x.UserQuizzes)
                .WithOne(x => x.Author)
                .HasForeignKey(x => x.AuthorId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(true);

            return builder;
        }

        public static ModelBuilder SetUpQuizForeignKeys(this ModelBuilder builder)
        {
            builder
                .Entity<Quiz>()
                .HasOne(x => x.Author)
                .WithMany(x => x.UserQuizzes)
                .HasForeignKey(x => x.AuthorId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(true);

            builder
                .Entity<Quiz>()
                .HasMany(x => x.Questions)
                .WithOne(x => x.Quiz)
                .HasForeignKey(x => x.QuizId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(true);

            return builder;
        }

        public static ModelBuilder SetUpQuestionForeignKeys(this ModelBuilder builder)
        {
            builder
                .Entity<Question>()
                .HasOne(x => x.Quiz)
                .WithMany(x => x.Questions)
                .HasForeignKey(x => x.QuizId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(true);

            builder
                .Entity<Question>()
                .HasMany(x => x.Answers)
                .WithOne(x => x.Question)
                .HasForeignKey(x => x.QuestionId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(true);

            return builder;
        }

        public static ModelBuilder SetUpAnswerForeignKeys(this ModelBuilder builder)
        {
            builder
                .Entity<Answer>()
                .HasOne(x => x.Question)
                .WithMany(x => x.Answers)
                .HasForeignKey(x => x.QuestionId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(true);

            return builder;
        }
    }
}