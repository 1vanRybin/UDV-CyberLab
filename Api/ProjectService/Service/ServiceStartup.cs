using Microsoft.Extensions.DependencyInjection;
using Service.AutoMapper;
using Service.Interfaces;
using Service.Services;

public static class ServiceStartUp
{
    public static IServiceCollection TryAddServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IProjectService, ProjectService>();
        serviceCollection.AddScoped<ICommentService, CommentService>();
        serviceCollection.AddScoped<IRatingService, RatingService>();
        serviceCollection.AddScoped<IAdminService, AdminService>();
        AddAutoMapper(serviceCollection);
        return serviceCollection;
    }

    private static void AddAutoMapper(IServiceCollection services)
    {
        services.AddAutoMapper(
            typeof(ProjectProfile),
            typeof(CommentProfile),
            typeof(RatingProfile));
    }
}