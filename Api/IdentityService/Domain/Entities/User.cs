using Core.BasicRoles;
using Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class User : IdentityUser<Guid>
    {
        [NotMapped]
        public UserRole Role { get; set; }

        public async Task<IdentityResult> CreateAsync(IUserStore userManager, string password, string userRole)
        {
            var createResult = await userManager.CreateAsync(this, password, userRole);
            return createResult;
        }
    }
}
