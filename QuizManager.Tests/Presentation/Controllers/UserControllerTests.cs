using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using QuizManager.Application.Services;
using QuizManager.Domain.Constants;
using QuizManager.Domain.Entities;
using QuizManager.Domain.Models.UserViewModels;
using QuizManager.Presentation.Controllers;
using System;
using System.Threading.Tasks;

using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace QuizManager.Test.Presentation.Controllers
{
    [TestFixture]
    internal class UserControllerTests
    {
        private Mock<SignInResult> signInResultMock;
        private SignInManagerMock signInManagerMock;
        private UserManagerMock userManagerMock;
        private Mock<IUserService> userServiceMock;

        private UserController sut;

        [SetUp]
        public void SetUp()
        {
            signInManagerMock = new SignInManagerMock();
            userManagerMock = new UserManagerMock();
            userServiceMock = new Mock<IUserService>();

            sut = new UserController(signInManagerMock, userManagerMock, userServiceMock.Object);
        }

        [Test]
        public async Task WhenLoggingIn_AndCredentialsInvalid_ThenReturnsLogInPage()
        {
            //Arrange
            signInManagerMock.ShouldSignInSucceed = false;

            //Act
            var result = await sut.Login(new LoginViewModel()) as ViewResult;

            //Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Model as LoginViewModel);
        }

        [Test]
        public async Task WhenLoggingIn_AndCredentialsValid_ThenRedirectsToUserDashboard()
        {
            //Arrange
            signInManagerMock.ShouldSignInSucceed = true;

            //Act
            var result = await sut.Login(new LoginViewModel()) as RedirectToRouteResult;

            //Assert
            Assert.NotNull(result);
            Assert.True(result.RouteName == RouteNames.GetUserDashboard);
        }
    }

    internal class SignInManagerMock : SignInManager<User>
    {
        public bool ShouldSignInSucceed { get; set; }

        public SignInManagerMock() : base(new UserManagerMock(),
                 new Mock<IHttpContextAccessor>().Object,
                 new Mock<IUserClaimsPrincipalFactory<User>>().Object,
                 new Mock<IOptions<IdentityOptions>>().Object,
                 new Mock<ILogger<SignInManager<User>>>().Object,
                 new Mock<IAuthenticationSchemeProvider>().Object,
                 new Mock<IUserConfirmation<User>>().Object)
        {
        }

        public override Task<SignInResult> PasswordSignInAsync(string userName, string password, bool isPersistent, bool lockoutOnFailure)
        {
            return Task.FromResult(ShouldSignInSucceed ? SignInResult.Success : SignInResult.Failed);
        }
    }

    internal class UserManagerMock : UserManager<User>
    {
        public UserManagerMock() : base(new Mock<IUserStore<User>>().Object,
              new Mock<IOptions<IdentityOptions>>().Object,
              new Mock<IPasswordHasher<User>>().Object,
              new IUserValidator<User>[0],
              new IPasswordValidator<User>[0],
              new Mock<ILookupNormalizer>().Object,
              new Mock<IdentityErrorDescriber>().Object,
              new Mock<IServiceProvider>().Object,
              new Mock<ILogger<UserManager<User>>>().Object)
        {
        }
    }
}