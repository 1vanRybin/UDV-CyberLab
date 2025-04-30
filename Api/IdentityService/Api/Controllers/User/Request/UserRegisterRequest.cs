using Core.BasicRoles;
using System.ComponentModel.DataAnnotations;

namespace IdentityServerApi.Controllers.User.Request;

public class UserRegisterRequest
{
    [Required]
    public string UserName { get; set; }
    
    [Required]
    public string Email { get; set; }
        
    [Required]
    public string Password { get; set; }
    
    [Required]
    public UserRole Role { get; set; }
}