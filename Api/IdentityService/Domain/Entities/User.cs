using System.Transactions;
using Core.BasicRoles;
using Domain.Interfaces;
using Medo;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities
{
    public class User : IdentityUser<Guid>
    {
        public async Task<IdentityResult> CreateAsync(IUserStore userManager, string password, string userRole)
        {
            var createResult = userManager.
            return createResult;
        }
    }
}
