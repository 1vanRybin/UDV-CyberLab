using System.Data.Entity;
using Core.BasicRoles;
using Domain.Entities;
using ExampleCore.AuthOptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Domain.Interfaces;

namespace Services.Services
{
    public class UserService : IUserService
    {
        private readonly IUserStore _userStore;
        private const int PageSize = 50;

        public UserService(IUserStore userStore)
        {
            _userStore = userStore;
        }

        public async Task<IdentityResult> RegisterUserAsync(User user, string password, UserRole role)
        {
            var createResult = await user.CreateAsync(_userStore, password, role.ToString());
            return createResult;
        }

        public async Task<JwtSecurityToken?> LoginUserAsync(User userLogin, string password)
        {
            var user = await _userManager.FindByNameAsync(userLogin.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, password))
            {
                return null;
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            var claims = CreateClaims(user, userRoles);
            var token = CreateToken(claims);
            
            return token;
        }
        
        public async Task<(User? user, UserRole role)> GetUserInfoAsync(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user is null)
            {
                return (null, default);
            }
            
            var roleString = (await _userManager.GetRolesAsync(user)).
                FirstOrDefault();
            
            var userRole = UserRole.User;
            if (roleString != null && roleString != UserRole.User.ToString() &&
                Enum.TryParse<UserRole>(roleString, out var parsedRole))
            {
                userRole = parsedRole;
            }
            
            return (user, userRole);
        }

        public async Task<List<User>> GetUsersAsync(int page)
        {
            var paginatedUsers = await _userStore.GetByPage(page);
            return paginatedUsers;
        }

        public async Task<Guid> CheckExistAsync(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            return user?.Id ?? Guid.Empty;
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
}
