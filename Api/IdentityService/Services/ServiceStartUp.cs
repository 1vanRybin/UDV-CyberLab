using Core.BasicRoles;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Services.Interfaces;
using Services.Services;

public static class ServiceStartUp
{
    public static IServiceCollection TryAddServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddKeyedScoped<IUserService, UserService>(UserRole.USER.ToString());
        serviceCollection.AddScoped<UserManager<User>>();
        serviceCollection.AddScoped<RoleManager<IdentityRole<Guid>>>();//либо убрать, либо переделать регистрацию ролей.

        return serviceCollection;
    }
}