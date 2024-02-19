namespace SQLServerAgent.API.Common.Options;

public class DatabaseConfiguration : IDatabaseConfiguration
{
    public string DefaultConnection { get; init; }
}