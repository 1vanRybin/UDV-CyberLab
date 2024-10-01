using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using Core.BasicRoles;

namespace Services.Interfaces
{
    public interface IUserService
    {
        Task<IdentityResult> RegisterUserAsync(User user, string Password, UserRole role);
        Task<JwtSecurityToken?> LoginUserAsync(User userLogin, string password);
        Task<(User? user, UserRole role)> GetUserInfoAsync(Guid userId);
        Task<List<User>> GetUsersAsync(int page);
    }
}
