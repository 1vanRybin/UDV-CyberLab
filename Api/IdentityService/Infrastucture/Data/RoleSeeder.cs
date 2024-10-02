using Core.BasicRoles;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastucture.Data;

public static class RoleSeeder
{
    public static async Task SeedRolesAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

        foreach (UserRole role in Enum.GetValues(typeof(UserRole)))
        {
            if (!await roleManager.RoleExistsAsync(role.ToString()))
            {
                await roleManager.CreateAsync(new IdentityRole<Guid>(role.ToString()));
            }
        }
    }
}