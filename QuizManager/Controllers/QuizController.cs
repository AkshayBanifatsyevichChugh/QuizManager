using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QuizManager.Application.Services;
using QuizManager.Domain.Constants;
using QuizManager.Domain.Entities;
using QuizManager.Domain.Enums;
using QuizManager.Domain.Models.QuizViewModels;
using QuizManager.Presentation.Extentions;
using System.Linq;
using System.Threading.Tasks;

namespace QuizManager.Presentation.Controllers
{
    [Authorize]
    [Route("Dashboard")]
    public class QuizController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly IQuizService _quizService;
        private readonly IUserService _userService;
        private readonly IMappingService _mappingService;

        public QuizController(IMappingService mappingService, UserManager<User> userManager, IQuizService quizService, IUserService userService)
        {
            _mappingService = mappingService;
            _userManager = userManager;
            _quizService = quizService;
            _userService = userService;
        }

        [HttpGet]
        [Route(nameof(CreateQuiz), Name = RouteNames.GetCreateQuiz)]
        public async Task<IActionResult> CreateQuiz()
        {
            if (User.Identity.IsAuthenticated)
            {
                var userIdentifier = User.Claims.GetUserIdentifier();
                var user = await _userService.GetUserAsync(userIdentifier);

                if (user.PermissionLevel != PermissionLevel.Edit)
                {
                    return Unauthorized();
                }

                return View();
            }

            return Unauthorized();
        }

        [HttpGet]
        [Route(nameof(AddQuestion), Name = RouteNames.GetAddQuestion)]
        public async Task<IActionResult> AddQuestion(long? quizId)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (quizId == null) { return BadRequest(); }

                var userIdentifier = User.Claims.GetUserIdentifier();
                var user = await _userService.GetUserAsync(userIdentifier);

                if (user.PermissionLevel != PermissionLevel.Edit){ return Unauthorized(); }

                var quiz = await _quizService.GetQuizAsync(quizId);

                if (quiz == null) { return BadRequest(); }

                var viewModel = _mappingService.MapAddQuestionViewModel(quiz, new Question(default, quiz.Id, null, null));

                return View(viewModel);
            }

            return Unauthorized();

        }

        [HttpGet]
        [Route(nameof(ViewQuiz), Name = RouteNames.GetViewQuiz)]
        public async Task<IActionResult> ViewQuiz(long? id)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (id == null) { return BadRequest(); }

                var userIdentifier = User.Claims.GetUserIdentifier();
                var user = await _userService.GetUserAsync(userIdentifier);

                var quiz = await _quizService.GetQuizAsync(id);

                if (quiz == null) { return BadRequest(); }

                var viewModel = _mappingService.MapViewQuizViewModel(user, quiz);

                return View(viewModel);
            }

            return Unauthorized();
        }

        [HttpGet]
        [Route(nameof(ViewQuestion), Name = RouteNames.GetViewQuestion)]
        public async Task<IActionResult> ViewQuestion(long? questionId)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (questionId == null) { return BadRequest(); };

                var userIdentifier = User.Claims.GetUserIdentifier();
                var user = await _userService.GetUserAsync(userIdentifier);

                if (user.PermissionLevel < PermissionLevel.View) { return Unauthorized(); }

                var question = await _quizService.GetQuestionAsync(questionId);

                var viewModel = _mappingService.MapViewQuestionViewModel(question);

                return View(viewModel);
            }

            return Unauthorized();
        }

        [HttpGet]
        [Route(nameof(DeleteQuiz), Name = RouteNames.GetDeleteQuiz)]
        public async Task<IActionResult> DeleteQuiz(long? id)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (id == null) { return BadRequest(); };

                var userIdentifier = User.Claims.GetUserIdentifier();
                var user = await _userService.GetUserAsync(userIdentifier);

                if (user.PermissionLevel != PermissionLevel.Edit)
                {
                    return Unauthorized();
                }

                await _quizService.DeleteQuizAsync(id);

                return RedirectToRoute(RouteNames.GetUserDashboard);
            }

            return Unauthorized();
        }

        [HttpGet]
        [Route(nameof(DeleteQuestion), Name = RouteNames.GetDeleteQuestion)]
        public async Task<IActionResult> DeleteQuestion(long? quizId, long? questionId)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (quizId == null || questionId == null) { return BadRequest(); };

                var userIdentifier = User.Claims.GetUserIdentifier();
                var user = await _userService.GetUserAsync(userIdentifier);

                if (user.PermissionLevel != PermissionLevel.Edit) { return Unauthorized(); }

                await _quizService.DeleteQuestionAsync(questionId);

                var quiz = await _quizService.GetQuizAsync(quizId);

                return RedirectToRoute(RouteNames.GetEditQuiz, new { id = quizId });
            }

            return Unauthorized();
        }

        [HttpGet]
        [Route(nameof(EditQuiz), Name = RouteNames.GetEditQuiz)]
        public async Task<IActionResult> EditQuiz(long? id)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (id == null) { return BadRequest(); };

                var userIdentifier = User.Claims.GetUserIdentifier();
                var user = await _userService.GetUserAsync(userIdentifier);

                if (user.PermissionLevel != PermissionLevel.Edit)
                {
                    return Unauthorized();
                }

                var quiz = await _quizService.GetQuizAsync(id);

                if (quiz == null) { return BadRequest(); }

                var viewModel = _mappingService.MapEditQuizViewModel(quiz);


                return View(viewModel);
            }

            return Unauthorized();
        }

        [HttpGet]
        [Route(nameof(EditQuestion), Name = RouteNames.GetEditQuestion)]
        public async Task<IActionResult> EditQuestion(long? quizId, long? questionId)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (quizId == null || questionId == null) { return BadRequest(); };

                var userIdentifier = User.Claims.GetUserIdentifier();
                var user = await _userService.GetUserAsync(userIdentifier);

                if (user.PermissionLevel != PermissionLevel.Edit){ return Unauthorized(); }

                var quiz = await _quizService.GetQuizAsync(quizId);

                if (quiz == null) { return BadRequest(); }

                var question = quiz.Questions.FirstOrDefault(x => x.Id == questionId);

                if (question == null) { return BadRequest(); }

                var viewModel = _mappingService.MapEditQuestionViewModel(quiz, question);

                return View(viewModel);
            }

            return Unauthorized();
        }

        [HttpPost]
        [Route(nameof(CreateQuiz), Name = RouteNames.PostCreateQuiz)]
        public async Task<IActionResult> CreateQuiz(CreateQuizViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                if (User.Identity.IsAuthenticated)
                {
                    var userIdentifier = User.Claims.GetUserIdentifier();
                    var user = await _userService.GetUserAsync(userIdentifier);

                    if (user.PermissionLevel != PermissionLevel.Edit)
                    {
                        return Unauthorized();
                    }

                    var quiz = _mappingService.MapCreateQuizViewModelToQuiz(viewModel);
                    quiz.AuthorId = user.Id;

                    await _quizService.CreateQuizAsync(quiz);

                    return RedirectToRoute(RouteNames.GetUserDashboard);
                }
            }

            return View(viewModel);
        }

        [HttpPost]
        [Route(nameof(AddQuestion), Name = RouteNames.PostAddQuestion)]
        public async Task<IActionResult> AddQuestion(AddQuestionViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                if (User.Identity.IsAuthenticated)
                {

                    if (viewModel.Question.QuizId == null) { return BadRequest(); }

                    var userIdentifier = User.Claims.GetUserIdentifier();
                    var user = await _userService.GetUserAsync(userIdentifier);

                    if (user.PermissionLevel != PermissionLevel.Edit){ return Unauthorized(); }

                    var quiz = await _quizService.GetQuizAsync(viewModel.Question.QuizId);

                    if (quiz == null) { return BadRequest(); }

                    var question = _mappingService.MapQuizQuestionViewModelToQuestion(viewModel.Question);

                    await _quizService.AddQuestion(question);

                    return RedirectToRoute(RouteNames.GetEditQuiz, new { Id = viewModel.Question.QuizId });
                }
            }

            return View(viewModel);
        }
    
        [HttpPost]
        [Route(nameof(EditQuestion), Name = RouteNames.PostEditQuestion)]
        public async Task<IActionResult> EditQuestion(EditQuestionViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                if (User.Identity.IsAuthenticated)
                {

                    if (viewModel.Question.QuizId == null) { return BadRequest(); }

                    var userIdentifier = User.Claims.GetUserIdentifier();
                    var user = await _userService.GetUserAsync(userIdentifier);

                    if (user.PermissionLevel != PermissionLevel.Edit){ return Unauthorized(); }

                    var question = await _quizService.GetQuestionAsync(viewModel.Question.Id, true);

                    if (question == null) { return BadRequest(); }

                    question.QuestionText = viewModel.Question.QuestionText;
                    question.Answers = _mappingService.MapQuizAnswerViewModelsToAnswers(viewModel.Question.Answers);

                    await _quizService.UpdateQuestionAsync(question);

                    return RedirectToRoute(RouteNames.GetEditQuiz, new { Id = viewModel.Question.QuizId });
                }
            }

            var quiz = await _quizService.GetQuizAsync(viewModel.Question.QuizId);
            
            viewModel.Quiz = quiz;

            return View(viewModel);
        }
    }
}