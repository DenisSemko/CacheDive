namespace SQLServerAgent.API.Common;

public static class AgentServicesRegistration
{
    public static IServiceCollection AddAgentService(this IServiceCollection services,
        ConfigurationManager configuration)
    {
        services.AddDbContext<ApplicationContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        
        services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        
        services.AddScoped<IJsonToTupleService, JsonToTupleService>();
        
        services.AddScoped<TransferJsonConsumer>();
        
        return services;
    }
}