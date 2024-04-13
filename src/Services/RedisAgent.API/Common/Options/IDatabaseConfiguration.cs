namespace RedisAgent.API.Common.Options;

public interface IDatabaseConfiguration
{
    string ConnectionString { get; init; }
}