var savedQuestions = [];
var currentQuestion;
function addAnswer(containerId) {
    let container = document.getElementById(containerId);
    let html = `
                <div class="input-group mb-3">
                    <div class="input-group-prepend">
                        <div class="input-group-text">
                            <input type="checkbox" title="check if this is the correct answer to the question" aria-label="Checkbox for following text input">
                            <label class="form-check-label ml-2" >Is correct</label>
                        </div>
                    </div>
                    <input class="form-control" title="type an answer for your question here" type="text" placeholder="Type an answer here" required>
                    <div class="input-group-append">
                        <button class="btn btn-danger btn-sm" type="button" title="delete answer" title="delete" onclick="deleteAnswer(this)">Delete</button>
                    </div>
                </div>
            `;
    container.insertAdjacentHTML('beforeend', html);
}
function saveQuestionToPreview(answersContainerId, questionTextId) {
    updateCurrentQuestion(answersContainerId, questionTextId);
    if (!validateCurrentQuestion(answersContainerId, questionTextId)) {
        return;
    }
    savedQuestions.push(currentQuestion);
    clearQuestion(answersContainerId, questionTextId);
    regenerateQuizPreview();
}
function saveQuestionToQuiz(answersContainerId, questionTextId, hiddenValuesContainerId, formId) {
    let form = document.getElementById(formId);
    updateCurrentQuestion(answersContainerId, questionTextId);
    if (!validateCurrentQuestion(answersContainerId, questionTextId)) {
        return;
    }
    regenerateHiddenVariablesForCurrentQuestion(hiddenValuesContainerId);
    form.submit();
}
function updateCurrentQuestion(answersContainerId, questionTextId) {
    let answersContainerElement = document.getElementById(answersContainerId);
    currentQuestion = { questionText: null, answers: [] };
    currentQuestion.questionText = document.getElementById(questionTextId).value;
    for (let i = 0; i < answersContainerElement.children.length; i++) {
        let answer = { answerText: null, isCorrectAnswer: null };
        let answerDiv = answersContainerElement.children[i];
        let answerTextElement = answerDiv.children[1];
        let answerIsCheckedElement = answerDiv.firstElementChild.firstElementChild.firstElementChild;
        answer.answerText = answerTextElement.value;
        answer.isCorrectAnswer = answerIsCheckedElement.checked;
        currentQuestion.answers.push(answer);
    }
}
function regenerateQuizPreview() {
    let quizPreviewContainer = document.getElementById("quiz-preview-container");
    quizPreviewContainer.innerHTML = null;
    for (let questionNumber = 0; questionNumber < savedQuestions.length; questionNumber++) {
        let questionPreviewHtml = generateQuizPreviewSingleQuestionHtml(questionNumber, savedQuestions[questionNumber]);
        quizPreviewContainer.innerHTML += questionPreviewHtml;
    }
}
function generateQuizPreviewSingleQuestionHtml(questionNumber, question) {
    let questionPreviewHtml = `
        <div id="question-preview-container-${questionNumber}">
            <input type="hidden" name="Questions[${questionNumber}].QuestionText" value="${question.questionText}">
            <button type="button" class="btn btn-danger btn-sm mr-1 float-right" title="delete question" onclick="deleteQuestion(${questionNumber}, this)">Delete</button>
            <li><p class="text-dark font-weight-bold">${question.questionText}</p></li>
            <ol type="A">            
`;
    for (let answerNumber = 0; answerNumber < savedQuestions[questionNumber].answers.length; answerNumber++) {
        questionPreviewHtml += `
            
        <input type="hidden" name="Questions[${questionNumber}].Answers[${answerNumber}].AnswerText" value="${question.answers[answerNumber].answerText}">
        <input type="hidden" name="Questions[${questionNumber}].Answers[${answerNumber}].IsCorrectAnswer" value="${question.answers[answerNumber].isCorrectAnswer}">`;
        if (question.answers[answerNumber].isCorrectAnswer) {
            questionPreviewHtml += `<li><p class="text-dark font-weight-bold">${question.answers[answerNumber].answerText} ✔</p></li>`;
        }
        else {
            questionPreviewHtml += `<li><p class="text-dark font-weight-bold">${question.answers[answerNumber].answerText}</p></li>`;
        }
    }
    questionPreviewHtml +=
        `
        </ol>
        </div>

        <br />
        `;
    return questionPreviewHtml;
}
function validateCurrentQuestion(answersContainerId, questionTextId) {
    let answersContainer = document.getElementById(answersContainerId);
    let questionTextElement = document.getElementById(questionTextId);
    if (questionTextElement.value.length < 1) {
        alert("Please add some question text.");
        return false;
    }
    if (answersContainer.children.length < 3) {
        alert("Please add at least 3 answers to your question.");
        return false;
    }
    if (answersContainer.children.length > 5) {
        alert("You can only have a maximum of 5 answers per question.");
        return false;
    }
    for (let i = 0; i < answersContainer.children.length; i++) {
        let answerDiv = answersContainer.children[i];
        let answerTextElement = answerDiv.children[1];
        answerTextElement.value = answerTextElement.value.replace(/^\s+/, '').replace(/\s+$/, '');
        if (!answerTextElement.checkValidity()) {
            alert("Please add text to all your answers.");
            return false;
        }
        if (!currentQuestion.answers.some(x => x.isCorrectAnswer)) {
            alert("Question must have at least one correct answer.");
            return false;
        }
    }
    return true;
}
function clearQuestion(answersContainerId, questionTextId) {
    let answersContainerElement = document.getElementById(answersContainerId);
    let questionTextElement = document.getElementById(questionTextId);
    answersContainerElement.innerHTML = null;
    questionTextElement.value = "";
}
function deleteAnswer(item) {
    item.parentElement.parentElement.remove();
}
function deleteQuestion(questionNumber, item) {
    savedQuestions.splice(questionNumber, 1);
    regenerateQuizPreview();
}
function regenerateHiddenVariablesForCurrentQuestion(containerId) {
    let container = document.getElementById(containerId);
    container.innerHTML = null;
    container.insertAdjacentHTML('beforebegin', generateHiddenValueHtml("QuestionText", currentQuestion.questionText));
    for (let i = 0; i < currentQuestion.answers.length; i++) {
        let currentAnswer = currentQuestion.answers[i];
        let isCorrectAnswer = currentAnswer.isCorrectAnswer;
        let answerText = currentAnswer.answerText;
        container.insertAdjacentHTML('beforebegin', generateHiddenValueHtml(`Question.Answers[${i}].IsCorrectAnswer`, isCorrectAnswer.toString()));
        container.insertAdjacentHTML('beforebegin', generateHiddenValueHtml(`Question.Answers[${i}].AnswerText`, answerText));
    }
}
function generateHiddenValueHtml(name, value) {
    return `<input type="hidden" name="${name}" value="${value}">`;
}
//# sourceMappingURL=site.js.map