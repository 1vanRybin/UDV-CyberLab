using Core.BasicRoles;
using System.ComponentModel.DataAnnotations;

namespace IdentityServerApi.Controllers.User.Request;

public class UserRegisterRequest
{
    [Required]
    [MinLength(5, ErrorMessage = "Min length must be 5 characters. ")]
    [MaxLength(20, ErrorMessage = "Max length must be 20 characters/")]
    public string FirstName { get; set; }

    [MinLength(5, ErrorMessage = "Min length must be 5 characters. ")]
    [MaxLength(20, ErrorMessage = "Max length must be 20 characters/")]
    public string SecondName { get; set; }
        
    [Required]
    public string Email { get; set; }
        
    [Required]
    public string Password { get; set; }
    public UserRoles Role { get; set; }
    public bool IsApproved { get; set; }
}