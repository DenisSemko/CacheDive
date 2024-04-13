namespace RedisAgent.API.Common.Options;

public class DatabaseConfiguration : IDatabaseConfiguration
{
    public string ConnectionString { get; init; }
}