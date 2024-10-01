using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Domain.Interfaces;

public interface IUserStore
{
    Task<IdentityResult> CreateAsync(User user, string password, string userRole);
    Task<List<User>> GetByPage(int page);
}