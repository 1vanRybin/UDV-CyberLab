using Domain.DTO;
using Domain.DTO.Answers;
using Domain.DTO.Questions;
using Domain.Entities;
using Domain.Interfaces;
using Service.interfaces;

namespace Service.Services
{
        public class TestPassingService : ITestPassingService
        {
            private readonly ITestStore _testRepository;
            private readonly IUserTestRepository _userTestRepository;
            private readonly IQuestionStore _questionRepository;
            private readonly IUserAnswerRepository _userAnswerRepository;

            public TestPassingService(
                ITestStore testRepository,
                IUserTestRepository userTestRepository,
                IQuestionStore questionRepository,
                IUserAnswerRepository userAnswerRepository)
            {
                _testRepository = testRepository;
                _userTestRepository = userTestRepository;
                _questionRepository = questionRepository;
                _userAnswerRepository = userAnswerRepository;
            }

        public async Task<Guid> StartTestAsync(Guid testId, Guid userId)
        {
            await СheckTestTime(testId);
            var userTest = await _userTestRepository.GetLatestByUserAndTestAsync(userId, testId);
            var test = await _testRepository.GetByIdAsync(testId);
            if (test is null)
            {
                throw new InvalidOperationException("Тест не найден.");
            }
            var leftTime = test.PassTestTime == null ?
                DateTime.UtcNow.AddDays(500) :
                DateTime.UtcNow.Add(test.PassTestTime.Value);

            if (userTest == null)
            {

                var newUserTest = new UserTest
                {
                    Id = Guid.NewGuid(),
                    TestId = testId,
                    TestName = test.Name,
                    UserId = userId,
                    AttemptNumber = 1,
                    LeftAttemptsCount = test.AttemptsCount - 1,
                    State = TestState.Running,
                    LeftTestTime = leftTime
                };

                await _userTestRepository.CreateAsync(newUserTest);

                return newUserTest.Id;
            }

            if (userTest.LeftAttemptsCount > 0)
            {
                var newUserTest = new UserTest
                {
                    Id = Guid.NewGuid(),
                    TestId = testId,
                    TestName = test.Name,
                    UserId = userId,
                    AttemptNumber = userTest.AttemptNumber+1,
                    LeftAttemptsCount = userTest.LeftAttemptsCount-1,
                    State = TestState.Running,
                    LeftTestTime = leftTime
                };
                await _userTestRepository.CreateAsync(newUserTest);

                return newUserTest.Id;
            }
            else
            {
                throw new InvalidOperationException("Нет доступных попыток для прохождения теста.");
            }
        }

            public async Task SaveAnswersAsync(Guid testId, Guid userId, UserAnswersDto userPreviewResultDto)
            {
                var userTest = await _userTestRepository.GetLatestByUserAndTestAsync(userId, testId);
                if (userTest == null || userTest.Id != userPreviewResultDto.UserTestId)
                {
                    throw new InvalidOperationException("Тест не был начат, не существует или не соответствует UserTestId.");
                }

                await СheckTestTime(testId);

                if (userPreviewResultDto.OpenAnswers != null)
                {
                    foreach (var openAnswer in userPreviewResultDto.OpenAnswers)
                    {
                        var question = await _questionRepository.GetByIdAsync(openAnswer.QuestionId);
                        if (question is not QuestionOpen questionOpen)
                        {
                            continue;
                        }

                        var userAnswer = new UserAnswer
                        {
                            UserTestId = userTest.Id,
                            QuestionId = openAnswer.QuestionId,
                            QuestionText = question.Text,
                            AnswerText = openAnswer.AnswerText,
                            CorrectText =  questionOpen.Answer
                        };

                        if (openAnswer.UserAnswerId != Guid.Empty)
                        {
                            userAnswer.Id = openAnswer.UserAnswerId;
                        }

                        await _userAnswerRepository.CreateOrUpdateAsync(userAnswer);
                    }
                }

                if (userPreviewResultDto.VariantAnswers != null)
                {
                    foreach (var variantAnswer in userPreviewResultDto.VariantAnswers)
                    {
                        var question = await _questionRepository.GetByIdAsync(variantAnswer.QuestionId);
                        if (question is not QuestionVariant questionVariant)
                        {
                            continue;
                        }

                        var userAnswer = new UserAnswer
                        {
                            UserTestId = userTest.Id,
                            QuestionId = variantAnswer.QuestionId,
                            QuestionText = question.Text,
                            VariantChoices = variantAnswer.SelectedAnswers,
                            CorrectChoices = questionVariant.CorrectAnswers
                        };

                        if (variantAnswer.UserAnswerId != Guid.Empty)
                        {
                            userAnswer.Id = variantAnswer.UserAnswerId;
                        }

                        await _userAnswerRepository.CreateOrUpdateAsync(userAnswer);
                    }
                }

                if (userPreviewResultDto.ComplianceAnswers != null)
                {
                    foreach (var complianceAnswer in userPreviewResultDto.ComplianceAnswers)
                    {
                        var question = await _questionRepository.GetByIdAsync(complianceAnswer.QuestionId);
                        if (question is not QuestionCompliance questionCompliance)
                        {
                            continue;
                        }

                        var userAnswer = new UserAnswer
                        {
                            UserTestId = userTest.Id,
                            QuestionText = question.Text,
                            QuestionId = complianceAnswer.QuestionId,
                            ComplianceData = complianceAnswer.UserCompliances,
                            CorrectData = questionCompliance.RightCompliances,
                        };

                        if (complianceAnswer.UserAnswerId != Guid.Empty)
                        {
                            userAnswer.Id = complianceAnswer.UserAnswerId;
                        }

                        await _userAnswerRepository.CreateOrUpdateAsync(userAnswer);
                    }
                }

                if (userPreviewResultDto.FileAnswers != null)
                {
                    foreach (var fileAnswer in userPreviewResultDto.FileAnswers)
                    {
                        var question = await _questionRepository.GetByIdAsync(fileAnswer.QuestionId);
                        if (question == null) continue;

                        var userAnswer = new UserAnswer
                        {
                            UserTestId = userTest.Id,
                            QuestionText = question.Text,
                            QuestionId = fileAnswer.QuestionId,
                            FileContent = fileAnswer.UserFileContent
                        };

                        if (fileAnswer.UserAnswerId != Guid.Empty)
                        {
                            userAnswer.Id = fileAnswer.UserAnswerId;
                        }

                        await _userAnswerRepository.CreateOrUpdateAsync(userAnswer);
                    }
                }
            }

            private async Task СheckTestTime(Guid testId)
            {
                var test = await _testRepository.GetByIdAsync(testId);
                if (test == null)
                    throw new InvalidOperationException("Тест не найден.");

                var now = DateTime.UtcNow;

                if (test.EndTestTime.HasValue && now > test.EndTestTime.Value)
                {
                    throw new InvalidOperationException("Время прохождения теста истекло. Ответы не сохраняются.");
                }
            }

            public async Task<UserAnswersDto> GetSavedAnswersAsync(Guid testId, Guid userId)
        {
            await СheckTestTime(testId);
            var userTest = await _userTestRepository.GetLatestByUserAndTestAsync(userId, testId);
            if (userTest == null)
            {
                throw new InvalidOperationException("UserTest not found or test not started.");
            }

            var answers = await _userAnswerRepository.GetAllByUserTestIdAsync(userTest.Id);

            var userAnswersDto = new UserAnswersDto
            {
                UserTestId = userTest.Id,
                OpenAnswers = new List<OpenAnswerDto>(),
                VariantAnswers = new List<VariantAnswerDto>(),
                ComplianceAnswers = new List<ComplianceAnswerDto>(),
                FileAnswers = new List<FileAnswerDto>()
            };

            foreach (var answer in answers)
            {
                var question = await _questionRepository.GetByIdAsync(answer.QuestionId);
                if (question == null) continue; if (question is QuestionOpen)
                {
                    userAnswersDto.OpenAnswers.Add(new OpenAnswerDto
                    {
                        UserAnswerId = answer.Id,
                        QuestionId = answer.QuestionId,
                        AnswerText = answer.AnswerText
                    });
                }
                else if (question is QuestionVariant)
                {
                    userAnswersDto.VariantAnswers.Add(new VariantAnswerDto
                    {
                        UserAnswerId = answer.Id,
                        QuestionId = answer.QuestionId,
                        SelectedAnswers = answer.VariantChoices ?? Array.Empty<int>()
                    });
                }
                else if (question is QuestionCompliance)
                {
                    userAnswersDto.ComplianceAnswers.Add(new ComplianceAnswerDto
                    {
                        UserAnswerId = answer.Id,
                        QuestionId = answer.QuestionId,
                        UserCompliances = answer.ComplianceData
                    });
                }
                else if (question is QuestionFile)
                {
                    userAnswersDto.FileAnswers.Add(new FileAnswerDto
                    {
                        UserAnswerId = answer.Id,
                        QuestionId = answer.QuestionId,
                        UserFileContent = answer.FileContent
                    });
                }
            }

            return userAnswersDto;
        }

            public async Task<FinishedTestResultDto> FinishTestAsync(Guid testId, Guid userId)
            {
                var userTest = await _userTestRepository.GetLatestByUserAndTestAsync(userId, testId);
                if (userTest == null || userTest.State == TestState.Completed)
                {
                    throw new InvalidOperationException("Тест не был начат или уже завершён.");
                }

                var finalScore = await CalculateScore(testId, userId);


                userTest.State = TestState.Completed;
                userTest.IsChecked = true;
                userTest.ScoredPoints = finalScore;
                await _userTestRepository.UpdateAsync(userTest);


                var finishedTestResult = await BuildFinishedTestResultDto(testId, userId, finalScore);

                return finishedTestResult;
            }


    private async Task<FinishedTestResultDto> BuildFinishedTestResultDto(Guid testId, Guid userId, float finalScore)
    {
        var test = await _testRepository.GetByIdAsync(testId);
        if (test == null)
            throw new InvalidOperationException("Тест не найден.");

        var userTest = await _userTestRepository.GetLatestByUserAndTestAsync(userId, testId);
        if (userTest == null)
            throw new InvalidOperationException("UserTest не найден.");

        var userAnswers = await _userAnswerRepository.GetAllByUserTestIdAsync(userTest.Id);

        var resultDto = new FinishedTestResultDto
        {
            Score = finalScore,
            MaxPoints = test.MaxPoints,
            Percentage = (test.MaxPoints > 0)
                ? (float)Math.Round(finalScore / test.MaxPoints * 100f, 2)
                : 0f
        };


        foreach (var question in test.Questions.OrderBy(q => q.SortOrder))
        {
            var questionResult = new QuestionResultDto
            {
                SortOrder = question.SortOrder,
                QuestionId = question.Id,
                QuestionText = question.Text
            };

            var userAnswer = userAnswers.FirstOrDefault(a => a.QuestionId == question.Id);

            if (question is QuestionOpen openQuestion)
            {
                var correct = openQuestion.Answer ?? string.Empty;
                var userValue = userAnswer?.AnswerText ?? string.Empty;

                questionResult.IsCorrect =
                    !string.IsNullOrWhiteSpace(correct) &&
                    correct.Equals(userValue.Trim(), StringComparison.OrdinalIgnoreCase);

                questionResult.UserAnswerText = userValue;
                questionResult.CorrectAnswerText = correct;
            }
            else if (question is QuestionVariant variantQuestion)
            {
                var correctSet = new HashSet<int>(variantQuestion.CorrectAnswers);
                var userSet = userAnswer?.VariantChoices != null
                    ? new HashSet<int>(userAnswer.VariantChoices)
                    : new HashSet<int>();

                questionResult.IsCorrect = userSet.SetEquals(correctSet);
                questionResult.UserChoices = userSet.ToArray();
                questionResult.CorrectChoices = correctSet.ToArray();
            }
            else if (question is QuestionCompliance complianceQuestion)
            {
                var correctDict = complianceQuestion.RightCompliances ?? new Dictionary<string, string>();
                var userDict = userAnswer?.ComplianceData ?? new Dictionary<string, string>();

                bool allRight = true;
                foreach (var kvp in correctDict)
                {
                    if (!userDict.TryGetValue(kvp.Key, out var userVal) ||
                        !userVal.Equals(kvp.Value, StringComparison.OrdinalIgnoreCase))
                    {
                        allRight = false;
                        break;
                    }
                }

                questionResult.IsCorrect = allRight;
                questionResult.UserCompliances = userDict;
                questionResult.CorrectCompliances = correctDict;
            }
            else if (question is QuestionFile fileQuestion)
            {
                questionResult.IsCorrect = (userAnswer != null && !string.IsNullOrEmpty(userAnswer.FileContent));
                questionResult.HasUserFile = questionResult.IsCorrect;
            }

            resultDto.Questions.Add(questionResult);
        }

        return resultDto;
    }

     private async Task<float> CalculateScore(Guid testId, Guid userId)
        {
            var test = await _testRepository.GetByIdAsync(testId);
            if (test == null || test.Questions == null || !test.Questions.Any())
                return 0;

            var userTest = await _userTestRepository.GetLatestByUserAndTestAsync(userId, testId);
            if (userTest == null)
                return 0;

            var userAnswers = await _userAnswerRepository.GetAllByUserTestIdAsync(userTest.Id);

            float totalScore = 0;

            foreach (var question in test.Questions)
            {
                if (question is QuestionOpen openQuestion)
                {
                    var userAnswer = userAnswers.FirstOrDefault(a => a.QuestionId == openQuestion.Id);
                    if (userAnswer != null &&
                        !string.IsNullOrWhiteSpace(openQuestion.Answer) &&
                        openQuestion.Answer.Equals(userAnswer.AnswerText?.Trim(), StringComparison.OrdinalIgnoreCase))
                    {
                        totalScore += openQuestion.Points;
                    }
                }
                else if (question is QuestionVariant variantQuestion)
                {
                    var userAnswer = userAnswers.FirstOrDefault(a => a.QuestionId == variantQuestion.Id);
                    if (userAnswer != null && userAnswer.VariantChoices != null)
                    {
                        var selectedSet = new HashSet<int>(userAnswer.VariantChoices);
                        var correctSet = new HashSet<int>(variantQuestion.CorrectAnswers);

                        if (selectedSet.SetEquals(correctSet))
                        {
                            totalScore += variantQuestion.Points;
                        }
                    }
                }
                else if (question is QuestionCompliance complianceQuestion)
                {
                    var rightDict = complianceQuestion.RightCompliances;
                    var userAnswer = userAnswers.FirstOrDefault(a => a.QuestionId == complianceQuestion.Id);
                    if (userAnswer != null && userAnswer.ComplianceData != null && rightDict != null)
                    {
                        var allRight = true;
                        foreach (var kvp in rightDict)
                        {
                            if (!userAnswer.ComplianceData.TryGetValue(kvp.Key, out var userVal) ||
                                !userVal.Equals(kvp.Value, StringComparison.OrdinalIgnoreCase))
                            {
                                allRight = false;
                                break;
                            }
                        }
                        if (allRight)
                        {
                            totalScore += complianceQuestion.Points;
                        }
                    }
                }
                else if (question is QuestionFile fileQuestion)
                {
                    var userAnswer = userAnswers.FirstOrDefault(a => a.QuestionId == fileQuestion.Id);
                    if (userAnswer != null && !string.IsNullOrEmpty(userAnswer.FileContent))
                    {
                        totalScore += fileQuestion.Points;
                    }
                }
            }

            if (test.MaxPoints > 0 && totalScore > test.MaxPoints)
            {
                totalScore = test.MaxPoints;
            }

            return totalScore;
        }
    }
}