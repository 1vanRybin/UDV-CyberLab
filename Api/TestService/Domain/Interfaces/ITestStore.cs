using Domain.Entities;

namespace Domain.Interfaces;

public interface ITestStore
{
    Task<ICollection<UserTest>> GetUserTestResultsAsync(Guid guid);

    Task<ICollection<QuestionBase>> GetAllQuestionsByTestIdAsync(Guid testId);
}