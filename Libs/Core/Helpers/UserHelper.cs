using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Http;

namespace ExampleCore.Helpers;

public static class UserHelper
{
    public static Guid GetUserId(HttpRequest request)
    {
        var token = request.Headers.Authorization.FirstOrDefault().ParseJwt();
        var userId = Guid.Parse(token.Claims.FirstOrDefault(c => c.Type == "id").Value);
        return userId;
    }
    
    private static JwtSecurityToken ParseJwt(this string jwt)
    {
        var token = jwt["Bearer ".Length..].Trim();
        var handler = new JwtSecurityTokenHandler();
        return handler.ReadJwtToken(token);
    }
}