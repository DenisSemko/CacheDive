namespace MongoAgent.API.Common.Options;

public interface IDbSettings
{
    string DatabaseName { get; set; }
    string ConnectionString { get; set; }
}