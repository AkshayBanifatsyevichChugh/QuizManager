using QuizManager.Domain.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace QuizManager.Presentation.Extentions
{
    public static class AuthenticationExtensions
    {
        public static Guid? GetUserIdentifier(this IEnumerable<Claim> claims)
        {
            var claim = claims.FirstOrDefault(x => x.Type == ClaimNames.UserIdentifier);

            return Guid.Parse(claim.Value);
        }
    }
}