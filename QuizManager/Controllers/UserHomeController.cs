using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizManager.Application.Services;
using QuizManager.Domain.Constants;
using QuizManager.Presentation.Extentions;
using System.Threading.Tasks;

namespace QuizManager.Presentation.Controllers
{
    [Authorize]
    [Route("Dashboard")]
    public class UserHomeController : Controller
    {
        private readonly IUserService _userService;
        private readonly IQuizService _quizService;
        private readonly IMappingService _mappingService;

        public UserHomeController(IUserService userService, IMappingService mappingService, IQuizService quizService)
        {
            _userService = userService;
            _mappingService = mappingService;
            _quizService = quizService;
        }

        [Route("", Name = RouteNames.GetUserDashboard)]
        public async Task<IActionResult> UserHome()
        {
            if (User.Identity.IsAuthenticated)
            {
                var userIdentifier = User.Claims.GetUserIdentifier();

                var user = await _userService.GetUserAsync(userIdentifier);
                var quizzes = await _quizService.GetQuizzesAsync();

                var viewModel = _mappingService.MapUserDashBoardViewModel(user, quizzes);

                return View(viewModel);
            }

            return View();
        }
    }
}