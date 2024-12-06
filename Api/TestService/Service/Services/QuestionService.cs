using Domain.Entities;
using Domain.Interfaces;
using ExampleCore.Dal.Base;
using Service.interfaces;

namespace WebApi.Services;

public class QuestionService : IQuestionService
{
    private readonly IStandartStore _repository;
    private readonly IQuestionStore _questionRepository;

    public QuestionService(IStandartStore repository, IQuestionStore questionRepository)
    {
        _repository = repository;
        _questionRepository = questionRepository;
    }
    
    public async Task<IQuestionBase> GetByIdAsync(Guid questionId)
    {
        var question = await _questionRepository.GetByIdAsync(questionId);
        
        return question switch
        {
            QuestionCompliance compliance => compliance,
            QuestionFile file => file,
            QuestionOpen open => open,
            QuestionVariant variant => variant,
            _ => null
        };
    }

    public async Task<Guid> CreateAsync<T>(T question) where T : BaseEntity<Guid>, IQuestionBase
    {
        var test =  await _repository.GetByIdAsync<Test>(question.TestId);
        if (test is null)
        {
            return Guid.Empty;
        }
        
        var result = await _repository.CreateAsync(question);
        
        return result;
    }
    
    public async Task<Guid> DeleteAsync(Guid questionId)
    {
        var target = await _repository.GetByIdAsync<QuestionBase>(questionId);
        if (target is null)
        {
            return Guid.Empty;
        }
        
        
        var result = await _repository.DeleteAsync(target);
        if (result)
        {
            return questionId;
        }
        
        return Guid.Empty;
    }

    public Task<T> UpdateAsync<T>(T question) where T : IQuestionBase
    {
        throw new NotImplementedException();
    }
}