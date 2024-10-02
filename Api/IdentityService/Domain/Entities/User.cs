using Domain.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities
{
    public class User : IdentityUser<Guid>
    {
        public async Task<IdentityResult> CreateAsync(IUserStore userManager, string password, string userRole)
        {
            var createResult = await userManager.CreateAsync(this, password, userRole);
            return createResult;
        }
    }
}
