
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IUserAnswerRepository
    {
        Task<Guid> CreateOrUpdateAsync(UserAnswer answer);
        Task<List<UserAnswer>> GetAllByUserTestIdAsync(Guid userTestId);
    }
}
