namespace ExperimentAgent.API.Common;

public static class AgentServicesRegistration
{
    public static IServiceCollection AddAgentService(this IServiceCollection services,
        ConfigurationManager configuration)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        
        services.AddScoped<IHandleExperimentRequestService, HandleExperimentRequestService>();
        
        services.AddEntityFrameworkNpgsql().AddDbContext<ApplicationContext>(options => 
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
        
        return services;
    }
}