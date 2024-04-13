namespace MongoAgent.API.Common;

public static class AgentServicesRegistration
{
    public static IServiceCollection AddAgentService(this IServiceCollection services, ConfigurationManager configuration)
    {
        return services;
    }
}