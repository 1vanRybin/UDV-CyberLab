using Core.BasicRoles;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;

namespace Domain.Interfaces;

public interface IUserStore
{
    Task<IdentityResult> CreateAsync(User user, string password, string userRole);
    Task<List<User>> GetByPageAsync(int page);
    Task<User?> CheckExistAsync(Guid id);
    Task<JwtSecurityToken?> LoginAsync(User userLogin, string password);
    Task<(User? user, UserRole role)> GetInfoAsync(Guid userId);
}