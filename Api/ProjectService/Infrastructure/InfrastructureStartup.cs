using Domain.Interfaces;
using Infrastructure;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

public static class InfrastuctureStartup
{
    public static IServiceCollection TryAddInfrastucture(this IServiceCollection serviceCollection, IConfigurationManager configurationManager)
    {        var connectionString = configurationManager.GetConnectionString("DefaultConnection");

        serviceCollection.TryAddScoped<IFileManager, FileManager>();
        serviceCollection.TryAddScoped<IProjectRepository, ProjectRepository>();

        serviceCollection.AddDbContext<ProjectsDbContext>(options =>
        {
            options.UseNpgsql(connectionString);
        });

        serviceCollection.AddScoped<DbContext>(provider => provider.GetService<ProjectsDbContext>());

        return serviceCollection;
    }
}