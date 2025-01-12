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

    public async Task<IEnumerable<TestDto>> GetAsync()
    {
        // todo ждем когда фронт скажет про погинацию и поменять на  _repository.GetPaginatedAsync<T>(int page, int pageSize)
        var testEntity = await _testStore.GetAllAsync();

        return _mapper.Map<IEnumerable<TestDto>>(testEntity);
    }

    public async Task<TestDto> GetByIdAsync(Guid id)
    {
        var testEntity = await _testStore.GetByIdAsync(id);
        if (testEntity is null)
        {
            return null;
        }
        
        var testDto = _mapper.Map<TestDto>(testEntity);

        testDto.Questions = await GetAllQuestionsByTestIdAsync(id);
        return testDto;
    }

    public async Task<ShortTestDto> GetByIdShortAsync(Guid id)
    {
        var testEntity = await _testStore.GetByIdShortAsync(id);
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
                ScoredPoints = 0,
                IsChecked = false
            };
        }

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

    public async Task<TestDto> UpdateAsync(Test test)
    {
        //todo подумать о том, что если тут вдруг ошибка будет.
        var testDto = await _repository.UpdateAsync(test);
        return _mapper.Map<TestDto>(testDto);
    }

    public async Task<ICollection<UserTestResultDto>?> GetUserTestResultsAsync(Guid userId)
    {
        var testResults = await _testStore.GetUserTestResultsAsync(userId);

        var testDto = _mapper.Map<ICollection<UserTestResultDto>>(testResults);

        return testDto;
    }

    public async Task<ICollection<object>> GetAllQuestionsByTestIdAsync(Guid testId)
    {
        var questions = await _testStore.GetAllQuestionsByTestIdAsync(testId);

        var res = new List<object>();
        foreach(var question in questions)
        {
            object? questionRes = question switch
            {
                QuestionCompliance compliance => compliance,
                QuestionFile file => file,
                QuestionOpen open => open,
                QuestionVariant variant => variant,
                _ => null
            };

            res.Add(questionRes);
        }

        return res; 
    }

    public async Task<List<Test?>> GetAllUserTestsAsync(Guid userId)
    {
        return await _testStore.GetAllUserTestsAsync(userId);
    }

    public async Task<List<UserTest?>> GetCompletedAsync(Guid userId)
    {
        return await _testStore.GetCompletedAsync(userId);
    }

    public async Task<List<UserTest?>> GetTestResultsAsync(Guid userId, Guid testId)
    {
        return await _testStore.GetTestResultsAsync(userId, testId);
    }

    public async Task<UserPreviewResultDto?> GetTestPreviewResult(Guid resultId)
    {
        var userTest = await _testStore.GetUserTest(resultId);
        if (userTest is null)
        {
            return null;
        }
        
        var userAnswer = await _userAnswerRepository.GetAllByUserTestIdAsync(resultId);
        var result = new UserPreviewResultDto
        {
            UserId = userTest.UserId,
            Questions = userAnswer.Select(answer => new QuestionResultDto
            {
                QuestionText = answer. QuestionText,
                QuestionId = answer.QuestionId,
                UserAnswerText = answer.AnswerText,
                CorrectAnswerText = answer.CorrectText,
                UserChoices = answer.VariantChoices,
                CorrectChoices = answer.CorrectChoices,
                UserCompliances = answer.ComplianceData,
                CorrectCompliances = answer.CorrectData,
                HasUserFile = !string.IsNullOrEmpty(answer.FileContent),
                IsCorrect = (answer.AnswerText == answer.CorrectText) 
                            && ((answer.VariantChoices == null && answer.CorrectChoices == null) || 
                                (answer.VariantChoices != null && answer.VariantChoices.SequenceEqual(answer.CorrectChoices))) 
                            && ((answer.ComplianceData == null && answer.CorrectData == null) || 
                                (answer.ComplianceData != null && answer.CorrectData != null 
                                                               && answer.ComplianceData.OrderBy(kv => kv.Key).SequenceEqual(answer.CorrectData.OrderBy(kv => kv.Key))))
            }).ToList()
        };
        
        
        return result;
    }
}
