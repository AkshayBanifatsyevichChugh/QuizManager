using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using QuizManager.Application.Services;
using QuizManager.Domain.Constants;
using QuizManager.Domain.Entities;
using QuizManager.Domain.Enums;
using QuizManager.Presentation.Controllers;
using QuizManager.Test.Presentation.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace QuizManager.Tests.Presentation.Controllers
{
    [TestFixture]
    public class QuizControllerTests
    {
        private Mock<User> user;
        private UserManagerMock userManager;
        private Mock<IQuizService> quizService;
        private Mock<IUserService> userService;
        private Mock<IMappingService> mappingService;
        private QuizController sut;

        [SetUp]
        public void SetUp()
        {
            user = new Mock<User>();
            userManager = new UserManagerMock();
            quizService = new Mock<IQuizService>();
            userService = new Mock<IUserService>();
            mappingService = new Mock<IMappingService>();
            sut = new QuizController(mappingService.Object, userManager, quizService.Object, userService.Object);
        }

        [Test]
        public async Task WhenAccessingCreateQuizPage_AndUserHasEditPermission_ThenPageIsLoaded()
        {
            //Arrange
            sut.ControllerContext = SetControllerContext(true);
            user.Object.PermissionLevel = PermissionLevel.Edit;
            userService
                .Setup(x => x.GetUserAsync(It.IsAny<Guid>()))
                .ReturnsAsync(user.Object);

            //Act
            var result = await sut.CreateQuiz() as ViewResult;

            //Assert
            Assert.NotNull(result);
        }

        [Test]
        public async Task WhenAccessingCreateQuizPage_AndUserHasRestrictedPermission_ThenReturns401()
        {
            //Arrange
            sut.ControllerContext = SetControllerContext(true);
            user.Object.PermissionLevel = PermissionLevel.Restricted;
            userService
                .Setup(x => x.GetUserAsync(It.IsAny<Guid>()))
                .ReturnsAsync(user.Object);

            //Act
            var result = await sut.CreateQuiz() as UnauthorizedResult;

            //Assert
            Assert.NotNull(result);
        }

        public ControllerContext SetControllerContext(bool isAuthenticated)
        {

            return new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                        {
                            new Claim(ClaimTypes.Name, "username"),
                            new Claim(ClaimNames.UserIdentifier, Guid.NewGuid().ToString())
                        },
                        isAuthenticated ? "auth" : "" ))
                }
            };
        }
    }
}
