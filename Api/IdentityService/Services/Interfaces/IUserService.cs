using System.IdentityModel.Tokens.Jwt;
using Core.BasicRoles;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Services.Interfaces
{
    public interface IUserService
    {
        Task<IdentityResult> RegisterUserAsync(User user, string Password, UserRole role);
        Task<JwtSecurityToken?> LoginUserAsync(User userLogin, string password);
        Task<User> GetUserInfoAsync(Guid userId);
        Task<List<User>> GetUsersAsync(string? name);
    }
}