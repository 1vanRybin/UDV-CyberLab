using System.IdentityModel.Tokens.Jwt;
using Core.BasicRoles;
using Core.Helpers;
using Domain.Entities;
using Domain.Interfaces;
using Medo;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastucture.Data;

public class UserRepository : IUserStore
{
    private const int PageSize = 50; //todo обсудить ограничение с фронтом
    private readonly ApplicationDbContext _dbContext;
    private readonly UserManager<User> _userManager;

    public UserRepository(UserManager<User> userManager,
        ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
        _userManager = userManager;
    }

    public async Task<IdentityResult> CreateAsync(User user, string password, string userRole)
    {
        await using var transaction = await _dbContext.Database.BeginTransactionAsync();
        user.Id = new Uuid7().ToGuid();
        var createResult = await _userManager.CreateAsync(user, password);
        if (!createResult.Succeeded)
        {
            return createResult;
        }

        var roleResult = await _userManager.AddToRoleAsync(user, userRole);
        if (!roleResult.Succeeded)
        {
            await transaction.RollbackAsync();
            return roleResult;
        }

        await transaction.CommitAsync();

        return createResult;
    }

    public async Task<JwtSecurityToken?> LoginAsync(User userLogin, string password)
    {
        var user = await _userManager.FindByEmailAsync(userLogin.Email);
        if (user == null || !await _userManager.CheckPasswordAsync(user, password))
        {
            return null;
        }

        var userRoles = await _userManager.GetRolesAsync(user);
        var claims = ClaimHelper.CreateClaims(user.Email, user.Id.ToString(), user.SecurityStamp, userRoles);
        var token = ClaimHelper.CreateToken(claims);

        return token;
    }

    public async Task<User> GetInfoAsync(Guid userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user is null)
        {
            return null;
        }

        var roleString = (await _userManager.GetRolesAsync(user)).FirstOrDefault();

        if (roleString != null && roleString != UserRole.USER.ToString() &&
            Enum.TryParse<UserRole>(roleString, out var parsedRole))
        {
            user.Role = parsedRole;
        }

        return user;
    }

    public async Task<IdentityResult> DeleteAsync(User user)
    {
        var result = await _userManager.DeleteAsync(user);

        return result;
    }

    public async Task<User?> FindByIdAsync(Guid userId)
    {
        var result = await _userManager.FindByIdAsync(userId.ToString());

        return result;
    }

    public async Task<List<User>> GetByPageAsync(int page)
    {
        var userQuery = _userManager.Users;

        var paginatedUsers = await userQuery
            .Skip((page - 1) * PageSize)
            .Take(PageSize)
            .ToListAsync();

        return paginatedUsers;
    }

    public async Task<User?> CheckExistAsync(Guid id)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());
        return user;
    }

    public async Task<List<User>> SearchByFilter(string? name)
    {
        var query = _dbContext.Users
            .Select(u => new User
            {
                Id = u.Id,
                UserName = u.UserName,
                Email = u.Email,
                Role = _dbContext.UserRoles.Where(ur => ur.UserId == u.Id).Join(
                        _dbContext.Roles,
                        ur => ur.RoleId,
                        r => r.Id,
                        (ur, r) => r.Name
                    ).Select(roleName =>
                        roleName == nameof(UserRole.ADMIN) ? UserRole.ADMIN :
                        roleName == nameof(UserRole.TEACHER) ? UserRole.TEACHER :
                        UserRole.USER
                    )
                    .FirstOrDefault()
            });

        if (!string.IsNullOrWhiteSpace(name)) query = query.Where(u => EF.Functions.ILike(u.UserName, $"%{name}%"));

        return await query.ToListAsync();
    }

    public async Task<List<User>> GetAllAsync()
    {
        var usersWithRoles = await _dbContext.Users
            .Select(u => new User
            {
                Id = u.Id,
                UserName = u.UserName,
                Email = u.Email,
                Role = _dbContext.UserRoles.Where(ur => ur.UserId == u.Id).Join(
                        _dbContext.Roles,
                        ur => ur.RoleId,
                        r => r.Id,
                        (ur, r) => r.Name
                    ).Select(roleName =>
                        roleName == nameof(UserRole.ADMIN) ? UserRole.ADMIN :
                        roleName == nameof(UserRole.TEACHER) ? UserRole.TEACHER :
                        UserRole.USER
                    )
                    .FirstOrDefault()
            })
            .ToListAsync();

        return usersWithRoles;
    }
}