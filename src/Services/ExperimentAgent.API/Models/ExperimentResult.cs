namespace ExperimentAgent.API.Models;

public class ExperimentResult
{
    public string Query { get; set; }
    public bool IsExecutedFromCache { get; set; }
    public int QueryExecutionNumber { get; set; }
    public double CacheHitRate { get; set; }
    public double CacheMissRate { get; set; }
    public string ExperimentExecutionTime { get; set; }
    public string Resources { get; set; }
    public double CacheSize { get; set; }
}