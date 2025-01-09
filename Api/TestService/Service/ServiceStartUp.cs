using Microsoft.Extensions.DependencyInjection;
using Service.AutoMapper;
using Service.interfaces;
using Service.Services;
using WebApi.Services;

public static class ServiceStartUp
{
    public static IServiceCollection TryAddServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<ITestService, TestsService>();
        serviceCollection.AddScoped<IQuestionService, QuestionService>();
        serviceCollection.AddScoped<ITestPassingService, TestPassingService>();
        AddAutoMapper(serviceCollection);
        return serviceCollection;
    }

    private static void AddAutoMapper(IServiceCollection services)
    {
        services.AddAutoMapper(
            typeof(QuestionsProfile),
            typeof(TestProfile));
    }
}