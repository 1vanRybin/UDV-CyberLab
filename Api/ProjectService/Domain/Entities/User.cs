using System.ComponentModel.DataAnnotations.Schema;
using Core.BasicRoles;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities;

public class User : IdentityUser<Guid>
{
    [NotMapped] public UserRole Role { get; set; }
}