using ExampleCore.AuthOptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
namespace Core.Helpers
{
    public static class ClaimHelper
    {
        public static List<Claim> CreateClaims(string email, string id, string stamp, IList<string> userRoles)
        {
            var claims = new List<Claim>()
            {
                new(ClaimTypes.Email, email),
                new("id", id),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(nameof(IdentityUser.SecurityStamp), stamp),
            };
            claims.AddRange(userRoles.Select(userRole =>
                new Claim(ClaimTypes.Role, userRole)));
            return claims;
        }

        public static JwtSecurityToken CreateToken(List<Claim> authClaims)
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
