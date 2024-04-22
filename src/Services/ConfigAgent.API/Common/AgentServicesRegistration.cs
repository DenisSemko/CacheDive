namespace ConfigAgent.API.Common;

public static class AgentServicesRegistration
{
    public static IServiceCollection AddAgentServices(this IServiceCollection services,
        ConfigurationManager configurationManager)
    {
        services.AddScoped<IValidateJsonService, ValidateJsonService>();
        services.AddScoped<IHandleJsonService, HandleJsonService>();
        
        return services;
    }
}