using System.ComponentModel.DataAnnotations;

namespace IdentityServerApi.Controllers.User.Request;

public class UserLoginRequest
{
    [Required]
    public string Email { get; set; }
    
    public string Password { get; set; }
}