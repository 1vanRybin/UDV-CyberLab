using Domain.DTO;
using Domain.DTO.Answers;
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

            public async Task StartTestAsync(Guid testId, Guid userId)
            {
                var userTest = await _userTestRepository.GetByUserAndTestAsync(userId, testId);
                if (userTest == null)
                {
                    var newUserTest = new UserTest
                    {
                        Id = Guid.NewGuid(),
                        TestId = testId,
                        UserId = userId,
                        AttemptNumber = 1,
                        LeftAttemptsCount = 3,
                        State = TestState.Running,
                        LeftTestTime = DateTime.UtcNow
                    };
                    await _userTestRepository.CreateAsync(newUserTest);
                }
                else
                {
                    if (userTest.LeftAttemptsCount > 0)
                    {
                        userTest.LeftAttemptsCount--;
                        userTest.AttemptNumber++;
                        userTest.State = TestState.Running;
                        userTest.LeftTestTime = DateTime.UtcNow;
                        await _userTestRepository.UpdateAsync(userTest);
                    }
                    else
                    {
                        throw new InvalidOperationException("Нет доступных попыток для прохождения теста.");
                    }
                }
            }

            public async Task SaveAnswersAsync(Guid testId, Guid userId, UserAnswersDto userAnswersDto)
            {
                var userTest = await _userTestRepository.GetByUserAndTestAsync(userId, testId);
                if (userTest == null || userTest.Id != userAnswersDto.UserTestId)
                {
                    throw new InvalidOperationException("Тест не был начат, не существует или не соответствует UserTestId.");
                }

                if (userAnswersDto.OpenAnswers != null)
                {
                    foreach (var openAnswer in userAnswersDto.OpenAnswers)
                    {
                        var question = await _questionRepository.GetByIdAsync(openAnswer.QuestionId);
                        if (question == null) continue;

                    var userAnswer = new UserAnswer
                    {
                        Id = Guid.NewGuid(),
                        UserTestId = userTest.Id,
                        QuestionId = openAnswer.QuestionId,
                        AnswerText = openAnswer.AnswerText
                    };
                        await _userAnswerRepository.CreateOrUpdateAsync(userAnswer);
                    }
                }

                if (userAnswersDto.VariantAnswers != null)
                {
                    foreach (var variantAnswer in userAnswersDto.VariantAnswers)
                    {
                        var question = await _questionRepository.GetByIdAsync(variantAnswer.QuestionId);
                        if (question == null) continue;

                        var userAnswer = new UserAnswer
                        {
                            Id = Guid.NewGuid(),
                            UserTestId = userTest.Id,
                            QuestionId = variantAnswer.QuestionId,
                            VariantChoices = variantAnswer.SelectedAnswers 
                        };
                        await _userAnswerRepository.CreateOrUpdateAsync(userAnswer);
                    }
                }

                if (userAnswersDto.ComplianceAnswers != null)
                {
                    foreach (var complianceAnswer in userAnswersDto.ComplianceAnswers)
                    {
                        var question = await _questionRepository.GetByIdAsync(complianceAnswer.QuestionId);
                        if (question == null) continue;

                        var userAnswer = new UserAnswer
                        {
                            Id = Guid.NewGuid(),
                            UserTestId = userTest.Id,
                            QuestionId = complianceAnswer.QuestionId,
                            ComplianceData = complianceAnswer.UserCompliances
                        };
                        await _userAnswerRepository.CreateOrUpdateAsync(userAnswer);
                    }
                }

                if (userAnswersDto.FileAnswers != null)
                {
                    foreach (var fileAnswer in userAnswersDto.FileAnswers)
                    {
                        var question = await _questionRepository.GetByIdAsync(fileAnswer.QuestionId);
                        if (question == null) continue;

                        var userAnswer = new UserAnswer
                        {
                            Id = Guid.NewGuid(),
                            UserTestId = userTest.Id,
                            QuestionId = fileAnswer.QuestionId,
                            FileContent = fileAnswer.UserFileContent
                        };
                        await _userAnswerRepository.CreateOrUpdateAsync(userAnswer);
                    }
                }
            }

        public async Task<UserAnswersDto> GetSavedAnswersAsync(Guid testId, Guid userId)
        {
            var userTest = await _userTestRepository.GetByUserAndTestAsync(userId, testId);
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
                        QuestionId = answer.QuestionId,
                        AnswerText = answer.AnswerText
                    });
                }
                else if (question is QuestionVariant)
                {
                    userAnswersDto.VariantAnswers.Add(new VariantAnswerDto
                    {
                        QuestionId = answer.QuestionId,
                        SelectedAnswers = answer.VariantChoices ?? Array.Empty<int>()
                    });
                }
                else if (question is QuestionCompliance)
                {
                    userAnswersDto.ComplianceAnswers.Add(new ComplianceAnswerDto
                    {
                        QuestionId = answer.QuestionId,
                        UserCompliances = answer.ComplianceData
                    });
                }
                else if (question is QuestionFile)
                {
                    userAnswersDto.FileAnswers.Add(new FileAnswerDto
                    {
                        QuestionId = answer.QuestionId,
                        UserFileContent = answer.FileContent
                    });
                }
            }

            return userAnswersDto;
        }

            public async Task<float> FinishTestAsync(Guid testId, Guid userId)
            {
                var userTest = await _userTestRepository.GetByUserAndTestAsync(userId, testId);
                if (userTest == null || userTest.State == TestState.Completed)
                {
                    throw new InvalidOperationException("Тест не был начат или уже завершён.");
                }

                float finalScore = await CalculateScore(testId, userId);

                userTest.State = TestState.Completed;
                userTest.IsChecked = true;
                userTest.ScoredPoints = finalScore;
                await _userTestRepository.UpdateAsync(userTest);

                return finalScore;
            }

     private async Task<float> CalculateScore(Guid testId, Guid userId)
        {
            // Находим сам тест и связанные вопросы
            var test = await _testRepository.GetByIdAsync(testId);
            if (test == null || test.Questions == null || !test.Questions.Any())
                return 0;

            // Получаем запись о прохождении
            var userTest = await _userTestRepository.GetByUserAndTestAsync(userId, testId);
            if (userTest == null)
                return 0;

            // Получаем все ответы пользователя
            var userAnswers = await _userAnswerRepository.GetAllByUserTestIdAsync(userTest.Id);

            float totalScore = 0;

            // Проходимся по всем вопросам теста и сравниваем ответы
            foreach (var question in test.Questions)
            {
                // Корректный ответ (в зависимости от типа вопроса):
                if (question is QuestionOpen openQuestion)
                {
                    // Найдём ответ пользователя
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
                    // Найдём ответ пользователя
                    var userAnswer = userAnswers.FirstOrDefault(a => a.QuestionId == variantQuestion.Id);
                    if (userAnswer != null && userAnswer.VariantChoices != null)
                    {
                        // Если варианты совпадают с CorrectAnswers (учитывая, что порядок не важен)
                        var selectedSet = new HashSet<int>(userAnswer.VariantChoices);
                        var correctSet = new HashSet<int>(variantQuestion.CorrectAnswers);
                        // Для примера: если выбран exactly correctSet => полный балл
                        // Иначе 0. Можно добавить частичное начисление баллов
                        if (selectedSet.SetEquals(correctSet))
                        {
                            totalScore += variantQuestion.Points;
                        }
                    }
                }
                else if (question is QuestionCompliance complianceQuestion)
                {
                    // Словарь правильных соответствий
                    var rightDict = complianceQuestion.RightCompliances;
                    var userAnswer = userAnswers.FirstOrDefault(a => a.QuestionId == complianceQuestion.Id);
                    if (userAnswer != null && userAnswer.ComplianceData != null && rightDict != null)
                    {
                        // Простейшая логика: если все пары ключ-значение совпадают => получаем все очки
                        var allRight = true;
                        foreach (var kvp in rightDict)
                        {
                            // kvp.Key есть ли у пользователя, и совпадает ли значение?
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
                        // Либо можно сделать частичное начисление (например, за каждое верное соответствие).
                    }
                }
                else if (question is QuestionFile fileQuestion)
                {
                    var userAnswer = userAnswers.FirstOrDefault(a => a.QuestionId == fileQuestion.Id);
                    if (userAnswer != null && !string.IsNullOrEmpty(userAnswer.FileContent))
                    {
                        // Например, просто добавляем баллы
                        totalScore += fileQuestion.Points;
                    }
                }
            }

            // Если тест имеет MaxPoints, можно ограничить результат (если нужно)
            if (test.MaxPoints > 0 && totalScore > test.MaxPoints)
            {
                totalScore = test.MaxPoints;
            }

            return totalScore;
        }
    }
}