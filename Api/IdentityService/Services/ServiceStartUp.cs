﻿using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Services.Interfaces;
using Services.Services;

public static class ServiceStartUp
{
    public static IServiceCollection TryAddServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IUserService, UserService>();
        serviceCollection.AddScoped<UserManager<User>>();
        serviceCollection.AddScoped<RoleManager<IdentityRole<Guid>>>();
        serviceCollection.AddScoped<IAdminService, AdminService>();

        return serviceCollection;
    }
}