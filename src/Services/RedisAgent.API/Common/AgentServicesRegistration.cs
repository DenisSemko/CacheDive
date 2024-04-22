using StackExchange.Redis;

namespace RedisAgent.API.Common;

public static class AgentServicesRegistration
{
    public static IServiceCollection AddAgentServices(this IServiceCollection services,
        ConfigurationManager configuration)
    {
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetValue<string>("RedisConfiguration:ConnectionString");
        });
        
        services.AddSingleton<IConnectionMultiplexer>(provider =>
        {
            return ConnectionMultiplexer.Connect(configuration.GetValue<string>("RedisConfiguration:ConnectionString"));
        });
        
        services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        
        services.AddScoped<IJsonToTupleService, JsonToTupleService>();
        
        services.Configure<DatabaseConfiguration>(configuration.GetSection("RedisConfiguration"));
        services.AddSingleton<IDatabaseConfiguration>(serviceProvider =>
            serviceProvider.GetRequiredService<IOptions<DatabaseConfiguration>>().Value);
        
        services.AddScoped<IExecutionQueryHelper, ExecutionQueryHelper>();
        services.AddScoped<IExecutionTimeHelper, ExecutionTimeHelper>();

        return services;
    }
}