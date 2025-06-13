using Domain.Entities;

namespace Services.Interfaces;

public interface IAdminService
{
    Task DeleteUserAsync(Guid userId);
    Task<List<User>> SearchUsersByNameAsync(string name);
}