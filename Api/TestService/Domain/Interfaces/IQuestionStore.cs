using Domain.Entities;

namespace Service.interfaces;

public interface IQuestionStore
{
    Task<QuestionBase?> GetByIdAsync(Guid id);
}