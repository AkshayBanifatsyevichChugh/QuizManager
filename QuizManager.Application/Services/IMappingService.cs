using QuizManager.Domain.Entities;
using QuizManager.Domain.Models.QuizViewModels;
using QuizManager.Domain.Models.UserHomeViewModels;
using System.Collections.Generic;
using System.Linq;

namespace QuizManager.Application.Services
{
    public interface IMappingService
    {
        AddQuestionViewModel MapAddQuestionViewModel(Quiz quiz, Question question);
        QuizAnswerViewModel MapAnswerToQuizAnswerViewModel(Answer answer);
        List<QuizAnswerViewModel> MapAnswersToQuizAnswerViewModels(List<Answer> answers);
        Quiz MapCreateQuizViewModelToQuiz(CreateQuizViewModel viewModel);
        EditQuestionViewModel MapEditQuestionViewModel(Quiz quiz, Question question);
        EditQuizViewModel MapEditQuizViewModel(Quiz quiz);
        List<Answer> MapQuizAnswerViewModelsToAnswers(List<QuizAnswerViewModel> viewModels);
        Answer MapQuizAnswerViewModelToAnswer(QuizAnswerViewModel viewModel);
        Question MapQuizQuestionViewModelToQuestion(QuizQuestionViewModel viewModel);
        QuizQuestionViewModel MapQuestionToQuizQuestionViewModel(Question question);
        List<Question> MapQuizQuestionViewModelToQuestion(List<QuizQuestionViewModel> viewModels);
        UserDashboardViewModel MapUserDashBoardViewModel(User user, List<Quiz> quizzes);
        ViewQuestionViewModel MapViewQuestionViewModel(Question question);
        ViewQuizViewModel MapViewQuizViewModel(User user, Quiz quiz);
    }

    public class MappingService : IMappingService
    {
        public AddQuestionViewModel MapAddQuestionViewModel(Quiz quiz, Question question)
        {
            return new AddQuestionViewModel(quiz, MapQuestionToQuizQuestionViewModel(question));
        }

        public List<QuizAnswerViewModel> MapAnswersToQuizAnswerViewModels(List<Answer> answers)
        {
            return answers?.Select(x => MapAnswerToQuizAnswerViewModel(x))?.ToList();
        }

        public QuizAnswerViewModel MapAnswerToQuizAnswerViewModel(Answer answer)
        {
            return new QuizAnswerViewModel(answer);
        }

        public Quiz MapCreateQuizViewModelToQuiz(CreateQuizViewModel viewModel)
        {
            return new Quiz(viewModel.Title, MapQuizQuestionViewModelToQuestion(viewModel.Questions));
        }

        public EditQuestionViewModel MapEditQuestionViewModel(Quiz quiz, Question question)
        {
            return new EditQuestionViewModel(quiz, MapQuestionToQuizQuestionViewModel(question));
        }

        public EditQuizViewModel MapEditQuizViewModel(Quiz quiz)
        {
            return new EditQuizViewModel(quiz);
        }

        public QuizQuestionViewModel MapQuestionToQuizQuestionViewModel(Question question)
        {
            return new QuizQuestionViewModel(question, MapAnswersToQuizAnswerViewModels(question.Answers));
        }

        public List<Answer> MapQuizAnswerViewModelsToAnswers(List<QuizAnswerViewModel> viewModels)
        {
            return viewModels.Select(x => MapQuizAnswerViewModelToAnswer(x)).ToList();
        }

        public Answer MapQuizAnswerViewModelToAnswer(QuizAnswerViewModel viewModel)
        {
            return new Answer(viewModel.Id, viewModel.AnswerText, viewModel.IsCorrectAnswer);
        }

        public Question MapQuizQuestionViewModelToQuestion(QuizQuestionViewModel viewModel)
        {
            return new Question(viewModel.Id, viewModel.QuizId, viewModel.QuestionText, MapQuizAnswerViewModelsToAnswers(viewModel.Answers));
        }

        public List<Question> MapQuizQuestionViewModelToQuestion(List<QuizQuestionViewModel> viewModels)
        {
            return viewModels.Select(x => MapQuizQuestionViewModelToQuestion(x)).ToList();
        }

        public UserDashboardViewModel MapUserDashBoardViewModel(User user, List<Quiz> quizzes)
        {
            return new UserDashboardViewModel(user, quizzes);
        }

        public ViewQuestionViewModel MapViewQuestionViewModel(Question question)
        {
            return new ViewQuestionViewModel(question);
        }

        public ViewQuizViewModel MapViewQuizViewModel(User user, Quiz quiz)
        {
            return new ViewQuizViewModel(user, quiz);
        }
    }
}