
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IUserAnswerRepository
    {
        Task CreateOrUpdateAsync(UserAnswer answer);
        Task<List<UserAnswer>> GetAllByUserTestIdAsync(Guid userTestId);
    }
}
