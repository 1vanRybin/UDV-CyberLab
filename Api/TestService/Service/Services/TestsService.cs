using AutoMapper;
using Domain.DTO;
using Domain.DTO.Answers;
using Domain.DTO.Questions;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Service.interfaces;

namespace Service.Services;

public class TestsService: ITestService
{
    private readonly IStandartStore _repository;
    private readonly ITestStore _testStore;
    private readonly IUserAnswerRepository _userAnswerRepository;
    private readonly IMapper _mapper;
    private readonly IQuestionStore _questionStore;


    public TestsService(IStandartStore repository, ITestStore testStore, IMapper mapper, IUserAnswerRepository userAnswerRepository, IQuestionStore questionStore)
    {
        _testStore = testStore;
        _repository = repository;
        _mapper = mapper;
        _userAnswerRepository = userAnswerRepository;
        _questionStore = questionStore;
    }

    public async Task<IEnumerable<TestDto>> GetAsync(string? difficulty = null, string? search = null, string? subject = null)
    {
        var testEntity = await _testStore.GetAllAsync(difficulty, search, subject);

        return _mapper.Map<IEnumerable<TestDto>>(testEntity);
    }

    public async Task<TestDto> GetByIdAsync(Guid id, bool isNeedAnswer)
    {
        var testEntity = await _testStore.GetByIdAsync(id);
        if (testEntity is null)
        {
            return null;
        }

        var testDto = _mapper.Map<TestDto>(testEntity);

        testDto.Questions = await GetAllQuestionsByTestIdAsync(id, isNeedAnswer);
        return testDto;
    }

    public async Task<UserTest> GetUserTestByIdAsync(Guid userTestId)
    {
        var userTest = await _repository.GetByIdAsync<UserTest>(userTestId);
        if (userTest is null)
        {
            return null;
        }

        return userTest;
    }

    public async Task<ShortTestDto> GetByIdShortAsync(Guid id, Guid  userId)
    {
        var testEntity = await _testStore.GetByIdAndUserIdShortAsync(id, userId);
        if (testEntity is null)
        {
            var test = await _testStore.GetByIdAsync(id);
            if (test is null)
            {
                return null;
            }

            return new ShortTestDto()
            {
                TestId = test.Id,
                Name = test.Name,
                Description = test.Description,
                Theme = test.Theme,
                Difficulty = test.Difficulty,
                OwnerId = test.OwnerId,
                StartTestTime = test.StartTestTime,
                EndTestTime = test.EndTestTime,
                State = TestState.Idle,
                AttemptNumber = 0,
                LeftAttemptsCount = test.AttemptsCount,
                MaxPoints = test.MaxPoints,
                PassTestTime = test.PassTestTime,
                ScoredPoints = 0,
                IsChecked = false
            };
        }

        if (testEntity.AttemptNumber < 0)
        {
            testEntity.AttemptNumber = 0;
        }

        var bestScore = await _testStore.GetBestScore(id, userId);
        testEntity.ScoredPoints = bestScore.ScoredPoints;
        var testDto = _mapper.Map<ShortTestDto>(testEntity);
        return testDto;
    }

    public async Task<Guid> CreateAsync(Test test)
    {
        var id = await _repository.CreateAsync(test);
        return id;
    }

    public async Task<TestDto?> DeleteAsync(Guid id)
    {
        var test = await _repository.GetByIdAsync<Test>(id);
        if (test is null)
        {
            return null;
        }

        var deleteResult = await _repository.DeleteAsync(test);

        return deleteResult ? _mapper.Map<TestDto>(test) : null;
    }

    public async Task<TestDto?> UpdateAsync(Test test)
    {
        var curTest = await _repository.GetByIdAsync<Test>(test.Id);
        if (curTest is null)
        {
            return null;
        }

        curTest.Name = test.Name;
        curTest.Theme = test.Theme;
        curTest.Description = test.Description;
        curTest.Difficulty = test.Difficulty;
        curTest.AttemptsCount = test.AttemptsCount;
        curTest.StartTestTime = test.StartTestTime;
        curTest.EndTestTime = test.EndTestTime;
        curTest.PassTestTime = test.PassTestTime;
        curTest.MaxPoints = test.MaxPoints;

        var testDto = await _repository.UpdateAsync(curTest);

        return _mapper.Map<TestDto>(testDto);
    }

    public async Task<ICollection<UserTestResultDto>?> GetUserTestResultsAsync(Guid userId)
    {
        var testResults = await _testStore.GetLastUserTests(userId);

        foreach (var lastTest in testResults)
        {
            var bestScore = await _testStore.GetBestScore(lastTest.TestId, userId);
            lastTest.ScoredPoints = bestScore.ScoredPoints;
        }

        var testDto = _mapper.Map<ICollection<UserTestResultDto>>(testResults);

        return testDto;
    }

    public async Task<ICollection<object>> GetAllQuestionsByTestIdAsync(Guid testId, bool isNeedAnswer)
    {
        var questions = await _testStore.GetAllQuestionsByTestIdAsync(testId);

        var res = new List<object>();
        foreach(var question in questions)
        {
            if (!isNeedAnswer)
            {
                switch (question)
                {
                    case QuestionCompliance compliance:
                        compliance.RightCompliances = null;
                        break;
                    case QuestionOpen open:
                        open.Answer = null;
                        break;
                    case QuestionVariant variant:
                        variant.CorrectAnswers = null;
                        break;
                }
            }

            res.Add(question);
        }

        return res;
    }

    public async Task<List<Test?>> GetAllUserTestsAsync(Guid userId)
    {
        return await _testStore.GetAllUserTestsAsync(userId);
    }

    public async Task<ICollection<UserTestResultDto?>> GetCompletedAsync(Guid userId)
    {
        var tests = await _testStore.GetCompletedAsync(userId);

        var testDto = _mapper.Map<ICollection<UserTestResultDto>>(tests);

        return testDto;
    }

    public async Task<List<UserTest?>> GetTestResultsAsync(Guid userId, Guid testId)
    {
        return await _testStore.GetTestResultsAsync(userId, testId);
    }

    public async Task<UserTestShortDto> GetUserTestResultAsync (Guid resultId)
    {
        var userTest = await _testStore.GetUserTest(resultId);
        var userAnswers = await _userAnswerRepository.GetAllByUserTestIdAsync(resultId);

        var result = new UserTestShortDto
        {
            Id = resultId,
            AttemptNumber = userTest.AttemptNumber,
            Questions = userTest.Test.Questions
            .Select(q =>
            new QuestionInfoDto
            {
                Id = q.Id,
                MaxPoints = q.Points,
                ScoredPoints = IsCorrect(userAnswers.Where(ua => ua.UserTestId == resultId && ua.QuestionId == q.Id).FirstOrDefault()) ? q.Points : 0
            })
            .ToList(),
            TestName = userTest.Test.Name,
            MaxPoints = userTest.Test.MaxPoints,
            ScoredPoints = userTest.ScoredPoints
        };

        return result;
    }

    public async Task<UserPreviewResultDto?> GetTestPreviewResult(Guid resultId)
    {
        var userTest = await _testStore.GetUserTest(resultId);
        if (userTest is null)
        {
            return null;
        }

        var test = await _testStore.GetByIdAsync(userTest.TestId);
        if (test is null)
        {
            return null;
        }

        var userAnswer = await _userAnswerRepository.GetAllByUserTestIdAsync(resultId);
        var result = new UserPreviewResultDto
        {
            UserId = userTest.UserId,
            CurrentPoints = userTest.ScoredPoints,
            MaxPoints = test.MaxPoints,
            Questions = userAnswer.Select(answer => new QuestionResultDto
            {

                QuestionText = answer.QuestionText,
                QuestionId = answer.QuestionId,
                UserAnswerText = answer.AnswerText,
                CorrectAnswerText = answer.CorrectText,
                UserChoices = answer.VariantChoices,
                CorrectChoices = answer.CorrectChoices,
                UserCompliances = answer.ComplianceData,
                CorrectCompliances = answer.CorrectData,
                HasUserFile = !string.IsNullOrEmpty(answer.FileContent),
                IsCorrect = IsCorrect(answer)
            }).ToList()
        };


        return result;
    }

    private static bool IsCorrect(UserAnswer? answer)
    {
        if (answer is null)
        {
            return false;
        }

        return (answer.AnswerText == answer.CorrectText)
                                    && ((answer.VariantChoices == null && answer.CorrectChoices == null) ||
                                        (answer.VariantChoices != null && new HashSet<int>(answer.VariantChoices).SetEquals(new HashSet<int>(answer.CorrectChoices))))
                                    && ((answer.ComplianceData == null && answer.CorrectData == null) ||
                                        (answer.ComplianceData != null && answer.CorrectData != null
                                                                       && answer.ComplianceData.OrderBy(kv => kv.Key).SequenceEqual(answer.CorrectData.OrderBy(kv => kv.Key))));
    }

    public async Task<List<StatisticsDto>> GetTestStatistics(Guid testId)
    {
        var userTests = await _testStore.GetTestStatistics(testId);
        var statistics = new List<StatisticsDto>();

        foreach(var test in userTests)
        {
            statistics.Add(new StatisticsDto
            {
                UserId = test.UserId,
                TestPoints = test.Test.MaxPoints,
                Points = test.ScoredPoints,
                UserTestId = test.Id,
                TestName = test.Test.Name
            });
        }

        return statistics;
    }
}
