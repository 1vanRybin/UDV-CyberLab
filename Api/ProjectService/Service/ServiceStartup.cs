using Microsoft.Extensions.DependencyInjection;
using Service.AutoMapper;
using Service.Interfaces;
using Service.Services;

public static class ServiceStartUp
{
    public static IServiceCollection TryAddServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IProjectService, ProjectService>();
        AddAutoMapper(serviceCollection);
        return serviceCollection;
    }

    private static void AddAutoMapper(IServiceCollection services)
    {
        services.AddAutoMapper(
            typeof(ProjectProfile));
    }
}