using AutoMapper;
using Domain.DTO.Questions;
using Domain.Entities;
using Domain.Interfaces;
using ExampleCore.Dal.Base;
using Service.interfaces;

namespace WebApi.Services;

public class QuestionService : IQuestionService
{
    private readonly IStandartStore _repository;
    private readonly IQuestionStore _questionRepository;
    private readonly IMapper _mapper;

    public QuestionService(
        IStandartStore repository,
        IQuestionStore questionRepository,
        IMapper mapper)
    {
        _repository = repository;
        _questionRepository = questionRepository;
        _mapper = mapper;
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
        test.MaxPoints += question.Points;
        await _repository.UpdateAsync(test);

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
            var test = await _repository.GetByIdAsync<Test>(target.TestId);
            test.MaxPoints -= target.Points;
            await _repository.UpdateAsync(test);

            return questionId;
        }

        return Guid.Empty;
    }

    public async Task<IQuestionBase> UpdateAsync(QuestionUpdateDto questionDto)
    {
        if (questionDto.ComplianceAnswer is not null)
        {
            await UpdatePoints(questionDto.ComplianceAnswer);
            return await _repository.UpdateAsync(_mapper.Map<QuestionCompliance>(questionDto.ComplianceAnswer));
        }

        if(questionDto.VariantAnswer is not null)
        {
            await UpdatePoints(questionDto.VariantAnswer);
            return await _repository.UpdateAsync(_mapper.Map<QuestionVariant>(questionDto.VariantAnswer));
        }

        if (questionDto.FileAnswer is not null)
        {
            await UpdatePoints(questionDto.FileAnswer);
            return await _repository.UpdateAsync(_mapper.Map<QuestionFile>(questionDto.FileAnswer));
        }

        if (questionDto.OpenAnswer is not null)
        {
            await UpdatePoints(questionDto.OpenAnswer);
            return await _repository.UpdateAsync(_mapper.Map<QuestionOpen>(questionDto.OpenAnswer));
        }

        return null;
    }

    public async Task UpdatePoints(QuestionBaseDto questionBase)
    {
        var test = await _repository.GetByIdAsync<Test>(questionBase.TestId);
        if (test is null)
        {
            return;
        }

        var question = await _repository.GetByIdAsync<QuestionBase>(questionBase.Id, true);
        if (question is null)
        {
            return;
        }

        test.MaxPoints = test.MaxPoints - question.Points + questionBase.Points;
        await _repository.UpdateAsync(test);
    }
}