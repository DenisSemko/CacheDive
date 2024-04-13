namespace ExperimentAgent.API.Entities;

public class ExperimentOutcome
{
    public Guid Id { get; set; }
    public string Query { get; set; }
    public DatabaseType DatabaseType { get; set; }
    public int QueryExecutionNumber { get; set; }
    public bool IsExecutedFromCache { get; set; }
    public double CacheHitRate { get; set; }
    public double CacheMissRate { get; set; }
    public string QueryExecutionTime { get; set; }
    public string Resources { get; set; }
    public double CacheSize { get; set; }
}