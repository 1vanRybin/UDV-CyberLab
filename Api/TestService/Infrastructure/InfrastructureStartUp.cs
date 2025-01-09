using Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Infrastucture.Data;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Service.interfaces;

public static class InfrastuctureStartUp
{
    public static IServiceCollection TryAddInfrastucture(this IServiceCollection serviceCollection, IConfigurationManager configurationManager)
    {
        serviceCollection.TryAddScoped<IStandartStore, BaseRepository>();
        serviceCollection.TryAddScoped<ITestStore, TestRepository>();
        serviceCollection.TryAddScoped<IQuestionStore, QuestionRepository>();
        serviceCollection.TryAddScoped<IUserAnswerRepository, UserAnswerRepository>();
        serviceCollection.TryAddScoped<IUserTestRepository, UserTestRepository>();

        var connectionString = configurationManager.GetConnectionString("DefaultConnection");

        serviceCollection.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(connectionString);
        });

        serviceCollection.AddScoped<DbContext>(provider => provider.GetService<ApplicationDbContext>());
        
        return serviceCollection;
    }
}