using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QuizManager.Application.Services;
using QuizManager.Domain.Constants;
using QuizManager.Domain.Entities;
using QuizManager.Presentation.Extentions;
using System.Threading.Tasks;

namespace QuizManager.Presentation.Controllers
{
    [Route("")]
    public class HomeController : Controller
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IUserService _userService;

        public HomeController(SignInManager<User> signInManager, UserManager<User> userManager, IUserService userService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _userService = userService;
        }

        [HttpGet]
        [Route("", Name = RouteNames.GetHome)]
        public async Task<IActionResult> Home()
        {
            if (User.Identity.IsAuthenticated)
            {
                var userIdentifier = User.Claims.GetUserIdentifier();

                var user = await _userService.GetUserAsync(userIdentifier);

                if (user != null)
                {
                    return RedirectToRoute(RouteNames.GetUserDashboard);
                }

                await _signInManager.SignOutAsync();

                return RedirectToRoute(RouteNames.GetHome);
            }
            return View();
        }
    }
}