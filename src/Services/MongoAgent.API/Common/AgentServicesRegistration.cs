namespace MongoAgent.API.Common;

public static class AgentServicesRegistration
{
    public static IServiceCollection AddAgentServices(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.Configure<DbSettings>(configuration.GetSection("ApplicationDatabase"));
        services.AddSingleton<IDbSettings>(serviceProvider =>
            serviceProvider.GetRequiredService<IOptions<DbSettings>>().Value);
        
        services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        
        services.AddScoped<IJsonToTupleService, JsonToTupleService>();
        
        services.AddScoped<IExecutionTimeHelper, ExecutionTimeHelper>();
        services.AddScoped<IExecutionQueryHelper, ExecutionQueryHelper>();
        
        return services;
    }
}