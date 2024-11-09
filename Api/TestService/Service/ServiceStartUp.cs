using Microsoft.Extensions.DependencyInjection;
using Service.interfaces;
using WebApi.Services;

public static class ServiceStartUp
{
    public static IServiceCollection TryAddServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<ITestService, TestsService>();

        return serviceCollection;
    }
}