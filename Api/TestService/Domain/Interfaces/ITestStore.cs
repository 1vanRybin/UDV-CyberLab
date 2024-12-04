using Domain.Entities;

namespace Domain.Interfaces;

public interface ITestStore
{
    Task<ICollection<TestResult>?> GetUserTestResultsAsync(Guid guid);
}