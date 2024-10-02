using System.Data.Entity;
using System.IdentityModel.Tokens.Jwt;
using Core.BasicRoles;
using Core.Helpers;
using Domain.Entities;
using Domain.Interfaces;
using Medo;
using Microsoft.AspNetCore.Identity;

namespace Infrastucture.Data;

public class UserRepository: IUserStore
{
    private readonly UserManager<User> _userManager;
    private readonly ApplicationDbContext _dbContext;
    private const int PageSize = 50;//todo обсудить ограничение с фронтом 
    
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

        var roleString = (await _userManager.GetRolesAsync(user)).
            FirstOrDefault();

        if (roleString != null && roleString != UserRole.USER.ToString() &&
            Enum.TryParse<UserRole>(roleString, out var parsedRole))
        {
            user.Role = parsedRole;
        }

        return user;
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

   
}
