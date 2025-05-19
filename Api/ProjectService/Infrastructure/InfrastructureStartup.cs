using Amazon.Runtime;
using Amazon.S3;
using Domain.Interfaces;
using ExampleCore.AuthOptions;
using Infrastructure;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

public static class InfrastuctureStartup
{
    public static IServiceCollection TryAddInfrastucture(this IServiceCollection serviceCollection,
        IConfigurationManager configurationManager)
    {
        var connectionString = configurationManager.GetConnectionString("DefaultConnection");
        serviceCollection.TryAddScoped<IProjectRepository, ProjectRepository>();
        serviceCollection.TryAddScoped<ICommentRepository, CommentRepository>();
        serviceCollection.TryAddScoped<IRatingRepository, RatingRepository>();
        serviceCollection.AddAuth();
        serviceCollection.AddDbContext<ProjectsDbContext>(options => { options.UseNpgsql(connectionString); });

        serviceCollection.AddScoped<DbContext>(provider => provider.GetService<ProjectsDbContext>());

        var awsOptions = new AmazonS3Config
        {
            ServiceURL = configurationManager["YandexStorage:ServiceUrl"]
        };

        serviceCollection.AddSingleton<IAmazonS3>(sp =>
            new AmazonS3Client(
                new BasicAWSCredentials(
                    configurationManager["YandexStorage:AccessKey"],
                    configurationManager["YandexStorage:SecretKey"]),
                awsOptions));

        serviceCollection.AddScoped<IFileManager, FileManager>();

        return serviceCollection;
    }
}