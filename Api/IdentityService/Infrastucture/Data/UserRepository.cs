using System.Data.Entity;
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

        return createResult;

    }
        
    public async Task<List<User>> GetByPage(int page)
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
