using Microsoft.AspNetCore.Identity;
using QuizManager.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QuizManager.Domain.Entities
{
    public class User : IdentityUser<long>
    {
        public User()
        {
            UserQuizzes = new List<Quiz>();
        }

        public User(string email, PermissionLevel permissionLevel, List<Quiz> userQuizzes)
        {
            Email = email;
            UserName = email;
            PermissionLevel = permissionLevel;

            UserIdentifier = Guid.NewGuid();
            
            UserQuizzes = userQuizzes ?? new List<Quiz>();
        }

        [Required]
        public Guid? UserIdentifier { get; set; }
        [Required]
        public PermissionLevel PermissionLevel { get; set; }

        public List<Quiz> UserQuizzes { get; set; }
    }
}