namespace SQLServerAgent.API.Common;

public static class AgentServicesRegistration
{
    public static IServiceCollection AddAgentServices(this IServiceCollection services,
        ConfigurationManager configuration)
    {
        services.AddDbContext<ApplicationContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        
        services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        
        services.AddScoped<IJsonToTupleService, JsonToTupleService>();
        
        services.AddScoped<JsonDataConsumer>();
        
        services.Configure<DatabaseConfiguration>(configuration.GetSection("ConnectionStrings"));
        services.AddSingleton<IDatabaseConfiguration>(serviceProvider =>
            serviceProvider.GetRequiredService<IOptions<DatabaseConfiguration>>().Value);
        
        services.AddScoped<IExecutionQueryHelper, ExecutionQueryHelper>();
        services.AddScoped<IExecutionTimeHelper, ExecutionTimeHelper>();
        
        return services;
    }
}