using Core.BasicRoles;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Infrastucture.Data;

public class RoleSeeder : IHostedService
{
    private readonly IServiceProvider _serviceProvider;

    public RoleSeeder(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task SeedRolesAsync()
    {
        using var scope = _serviceProvider.CreateScope();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

        foreach (UserRole role in Enum.GetValues(typeof(UserRole)))
        {
            if (!await roleManager.RoleExistsAsync(role.ToString()))
            {
                await roleManager.CreateAsync(new IdentityRole<Guid>(role.ToString()));
            }
        }
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await SeedRolesAsync();
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}