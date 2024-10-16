using Domain.Entities;
using Infrastucture.Data;

namespace WebApi.Services;

public class TestsService
{
    private readonly BaseRepository _repository;

    public TestsService(BaseRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Test>> Get()
    {
        var res = await _repository.GetAllAsync<Test>();
        return res;
    }

    public async Task<Test> GetById(Guid id)
    {
        var res = await _repository.GetByIdAsync<Test>(id);
        return res;
    }

    public async Task<Guid> Create(Test test)
    {
        var id = await _repository.CreateAsync(test);
        return id;
    }
    
    public async Task Delete(Guid id)
    {
        var test = await _repository.GetByIdAsync<Test>(id);
        await _repository.DeleteAsync(test);
    }
}