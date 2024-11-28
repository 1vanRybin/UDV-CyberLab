using Domain.Entities;
using Domain.Interfaces;
using Service.interfaces;

namespace WebApi.Services;

public class TestsService: ITestService
{
    private readonly IStandartStore _repository;

    public TestsService(IStandartStore repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Test>> GetAsync()
    {
        // todo ждем когда фронт скажет про погинацию и поменять на  _repository.GetPaginatedAsync<T>(int page, int pageSize)
        var res = await _repository.GetAllAsync<Test>();
        return res;
    }

    public async Task<Test?> GetByIdAsync(Guid id)
    {
        var res = await _repository.GetByIdAsync<Test>(id);
        return res;
    }

    public async Task<Guid> CreateAsync(Test test)
    {
        var id = await _repository.CreateAsync(test);
        return id;
    }
    
    public async Task<Test?> DeleteAsync(Guid id)
    {
        var test = await _repository.GetByIdAsync<Test>(id);
        if (test is null) 
        {
            return null;
        }
        
        var deleteResult = await _repository.DeleteAsync(test);

        return deleteResult ? test : null;
    }
}