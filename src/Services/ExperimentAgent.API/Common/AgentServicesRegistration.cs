namespace ExperimentAgent.API.Common;

public static class AgentServicesRegistration
{
    public static IServiceCollection AddAgentService(this IServiceCollection services,
        ConfigurationManager configuration)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        
        services.AddEntityFrameworkNpgsql().AddDbContext<ApplicationContext>(options => 
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
        
        services.AddScoped<IHandleExperimentRequestService, HandleExperimentRequestService>();
        
        services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        
        return services;
    }
}