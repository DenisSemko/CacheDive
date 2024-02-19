namespace SQLServerAgent.API.Common.Options;

public interface IDatabaseConfiguration
{
    string DefaultConnection { get; init; }
}