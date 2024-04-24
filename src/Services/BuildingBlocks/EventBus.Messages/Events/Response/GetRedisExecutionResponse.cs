namespace EventBus.Messages.Events.Response;

public class GetRedisExecutionResponse : IntegrationBaseEvent
{
    public string Query { get; set; } = "Query is not applied";
    public int QueryExecutionNumber { get; set; }
    public bool IsExecutedFromCache { get; set; } = true;
    public double CacheHitRate { get; set; }
    public double CacheMissRate { get; set; }
    public string ExperimentExecutionTime { get; set; }
    public string Resources { get; set; }
    public double CacheSize { get; set; }
}