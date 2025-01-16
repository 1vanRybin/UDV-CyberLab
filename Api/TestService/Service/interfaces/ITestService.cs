using Domain.DTO;
using Domain.DTO.Answers;
using Domain.DTO.Questions;
using Domain.Entities;

namespace Service.interfaces;

public interface ITestService
{
    Task<IEnumerable<TestDto>> GetAsync();

    Task<TestDto> GetByIdAsync(Guid id);
    Task<ShortTestDto> GetByIdShortAsync(Guid id, Guid userId);
    Task<UserTest> GetUserTestByIdAsync(Guid userTestId);

    Task<Guid> CreateAsync(Test test);

    Task<TestDto?> DeleteAsync(Guid id);

    Task<TestDto?> UpdateAsync(Test test);

    Task<ICollection<UserTestResultDto>?> GetUserTestResultsAsync(Guid userId);
    Task<ICollection<object>> GetAllQuestionsByTestIdAsync(Guid testId);
    Task<List<Test?>> GetAllUserTestsAsync(Guid userId);
    Task<ICollection<UserTestResultDto?>> GetCompletedAsync(Guid userId);
    Task<List<UserTest?>> GetTestResultsAsync(Guid userId, Guid testId);
    Task<UserPreviewResultDto?> GetTestPreviewResult(Guid resultId);
    Task<List<StatisticsDto>> GetTestStatistics(Guid testId);
}