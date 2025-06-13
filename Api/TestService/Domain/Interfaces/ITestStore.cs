using Domain.Entities;

namespace Domain.Interfaces;

public interface ITestStore
{
    Task<Test?> GetByIdAsync(Guid testId);
    Task<UserTest?> GetByIdAndUserIdShortAsync(Guid testId, Guid userId);
    Task<UserTest?> GetBestScore(Guid testId, Guid userId);
    Task<ICollection<UserTest?>> GetUserTestResultsAsync(Guid guid);
    Task<ICollection<UserTest?>> GetLastUserTests(Guid userId);
    Task<ICollection<QuestionBase>> GetAllQuestionsByTestIdAsync(Guid testId);
    Task<List<Test?>> GetAllAsync(string? difficulty = null, string? search = null, string? subject = null);
    Task<List<Test?>> GetAllUserTestsAsync(Guid userId);
    Task<List<UserTest?>> GetCompletedAsync(Guid userId);
    Task<List<UserTest?>> GetTestResultsAsync(Guid userId, Guid testId);
    Task<UserTest?> GetUserTest(Guid resultId);
    Task<List<UserTest?>> GetTestStatistics(Guid testId);
}