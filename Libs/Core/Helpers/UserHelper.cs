using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Http;

namespace ExampleCore.Helpers;

public static class UserHelper
{
    public static Guid GetUserId(HttpRequest request)
    {
        var userId = Guid.Parse("0a227578-aae3-4aca-af25-4542e930fab3");
        return userId;
    }
    
    private static JwtSecurityToken ParseJwt(this string jwt)
    {
        var token = jwt["Bearer ".Length..].Trim();
        var handler = new JwtSecurityTokenHandler();
        return handler.ReadJwtToken(token);
    }
}