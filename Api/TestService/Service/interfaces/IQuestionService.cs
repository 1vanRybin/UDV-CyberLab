
using Domain.Entities;
using Domain.Interfaces;
using ExampleCore.Dal.Base;

namespace Service.interfaces
{
    public interface IQuestionService
    {
        public Task<QuestionBase?> GetByIdAsync(Guid questionId);
        public Task<IReadOnlyList<IQuestionBase>> GetByTestIdAsync(Guid testId);

        public Task<Guid> CreateAsync<T>(T question)
            where T : BaseEntity<Guid>, IQuestionBase;
        public Task<Guid> DeleteAsync(Guid questionId);
        Task<T> UpdateAsync<T>(T question)
            where T: IQuestionBase;
    }
}
