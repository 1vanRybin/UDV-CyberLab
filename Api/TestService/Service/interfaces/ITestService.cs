using Domain.DTO;
using Domain.DTO.Answers;
using Domain.DTO.Questions;
using Domain.Entities;

namespace Service.interfaces;

public interface ITestService
{
    Task<IEnumerable<TestDto>> GetAsync();

    Task<TestDto> GetByIdAsync(Guid id);
    Task<ShortTestDto> GetByIdShortAsync(Guid id);

    Task<Guid> CreateAsync(Test test);

    Task<TestDto?> DeleteAsync(Guid id);

    Task<TestDto> UpdateAsync(Test test);

    Task<ICollection<UserTestResultDto>?> GetUserTestResultsAsync(Guid userId);
    Task<ICollection<object>> GetAllQuestionsByTestIdAsync(Guid testId);
    Task<List<Test?>> GetAllUserTestsAsync(Guid userId);
    Task<List<UserTest?>> GetCompletedAsync(Guid userId);
    Task<List<UserTest?>> GetTestResultsAsync(Guid userId, Guid testId);
    Task<UserPreviewResultDto?> GetTestPreviewResult(Guid resultId);
}