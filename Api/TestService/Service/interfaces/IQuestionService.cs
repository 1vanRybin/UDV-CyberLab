
using Domain.DTO.Questions;
using Domain.Entities;
using Domain.Interfaces;
using ExampleCore.Dal.Base;

namespace Service.interfaces
{
    public interface IQuestionService
    {
        public Task<IQuestionBase> GetByIdAsync(Guid questionId);

        public Task<Guid> CreateAsync<T>(T question)
            where T : BaseEntity<Guid>, IQuestionBase;
        public Task<Guid> DeleteAsync(Guid questionId);
        Task<IQuestionBase> UpdateAsync(QuestionUpdateDto questionDto);
    }
}
