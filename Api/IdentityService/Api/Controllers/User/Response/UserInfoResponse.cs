using Core.BasicRoles;

namespace IdentityServerApi.Controllers.User.Request;


public class UserInfoResponse
{
    public Guid UserId { get; set; }
    public string UserName { get; set; }
    
    public string Email { get; set; }
    
    public UserRole Role { get; set; }
}