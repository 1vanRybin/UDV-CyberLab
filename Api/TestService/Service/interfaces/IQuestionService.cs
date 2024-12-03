
using Domain.Interfaces;

namespace Service.interfaces
{
    public interface IQuestionService
    {
        public Task<IQuestionBase> GetQuestionByIdAsync(Guid questionId);
        public Task<IReadOnlyList<IQuestionBase>> GetQuestionsByTestIdAsync(Guid questionId);
        public Task<Guid> PostQuestionAsync<T>(T question)
            where T : IQuestionBase;
        public Task<T> PutQuestionAsync<T>(T question)
            where T : IQuestionBase;
        public Task<Guid> DeleteQuestionAsync(Guid questionId);
    }
}
