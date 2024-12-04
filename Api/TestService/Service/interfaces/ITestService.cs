using Domain.Entities;

namespace Service.interfaces;

public interface ITestService
{
    public Task<IEnumerable<Test>> GetAsync();

    public Task<Test?> GetByIdAsync(Guid id);

    public Task<Guid> CreateAsync(Test test);

    public Task<Test?> DeleteAsync(Guid id);
    
    public Task<Test> UpdateAsync(Test test);

    public Task<ICollection<TestResult>?> GetUserTestResultsAsync(Guid userId);
}