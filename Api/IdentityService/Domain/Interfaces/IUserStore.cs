using System.IdentityModel.Tokens.Jwt;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Domain.Interfaces;

public interface IUserStore
{
    Task<IdentityResult> CreateAsync(User user, string password, string userRole);
    Task<List<User>> GetByPageAsync(int page);
    Task<User?> CheckExistAsync(Guid id);
    Task<JwtSecurityToken?> LoginAsync(User userLogin, string password);
    Task<User> GetInfoAsync(Guid userId);
    Task<IdentityResult> DeleteAsync(User user);
    Task<User?> FindByIdAsync(Guid userId);
    Task<List<User>> SearchByFilter(string? name);
}