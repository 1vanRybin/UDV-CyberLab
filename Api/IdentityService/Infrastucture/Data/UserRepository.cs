using System.Data.Entity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Core.BasicRoles;
using Domain.Entities;
using Domain.Interfaces;
using ExampleCore.AuthOptions;
using Medo;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

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
        var claims = CreateClaims(user, userRoles);
        var token = CreateToken(claims);

        return token;
    }

    public async Task<(User? user, UserRole role)> GetInfoAsync(Guid userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user is null)
        {
            return (null, default);
        }

        var roleString = (await _userManager.GetRolesAsync(user)).
            FirstOrDefault();

        var userRole = UserRole.USER;
        if (roleString != null && roleString != UserRole.USER.ToString() &&
            Enum.TryParse<UserRole>(roleString, out var parsedRole))
        {
            userRole = parsedRole;
        }

        return (user, userRole);
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

    private static List<Claim> CreateClaims(User user, IList<string> userRoles)
    {
        var claims = new List<Claim>()
            {
                new(ClaimTypes.Email, user.Email),
                new("id", user.Id.ToString()),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(nameof(IdentityUser.SecurityStamp), user.SecurityStamp),
            };
        claims.AddRange(userRoles.Select(userRole =>
            new Claim(ClaimTypes.Role, userRole)));
        return claims;
    }

    private static JwtSecurityToken CreateToken(List<Claim> authClaims)
    {
        var authSigningKey = AuthOptions.GetSymmetricSecurityKey();

        var token = new JwtSecurityToken(
            issuer: AuthOptions.ISSUER,
            audience: AuthOptions.AUDIENCE,
            expires: DateTime.Now.AddHours(3),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        );

        return token;
    }
}
