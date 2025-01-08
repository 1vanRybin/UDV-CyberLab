using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IUserTestRepository
    {
        Task<UserTest?> GetByUserAndTestAsync(Guid userId, Guid testId);
        Task CreateAsync(UserTest userTest);
        Task UpdateAsync(UserTest userTest);
    }
}
