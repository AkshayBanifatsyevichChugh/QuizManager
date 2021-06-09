using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QuizManager.Application.Services;
using QuizManager.Domain.Constants;
using QuizManager.Domain.Entities;
using QuizManager.Domain.Models.UserViewModels;
using System.Threading.Tasks;

namespace QuizManager.Presentation.Controllers
{
    [Route("")]
    public class UserController : Controller
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IUserService _userService;

        public UserController(SignInManager<User> signInManager, UserManager<User> userManager, IUserService userService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _userService = userService;
        }

        [Route(nameof(Login), Name = RouteNames.GetLogin)]
        public IActionResult Login()
        {
            return View();
        }

        [Authorize]
        [HttpGet]
        [Route(nameof(Logout), Name = RouteNames.GetLogout)]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToRoute(RouteNames.GetLogin);
        }

        [HttpPost]
        [Route(nameof(Login), Name = RouteNames.PostLogin)]
        public async Task<IActionResult> Login(LoginViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(viewModel.Email, viewModel.Password, false, false);

                if (result.Succeeded)
                {
                    return RedirectToRoute(RouteNames.GetUserDashboard);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid credentials.");
                }
            }

            return View(viewModel);
        }
    }
}