namespace EventBus.Messages.Events.Response;

public class GetSqlServerExecutionResponse : IntegrationBaseEvent
{
    public ExperimentType ExperimentType { get; set; }
    public string Query { get; set; }
    public bool IsExecutedFromCache { get; set; }
    public int QueryExecutionNumber { get; set; }
    public double CacheHitRate { get; set; }
    public double CacheMissRate { get; set; }
    public string ExperimentExecutionTime { get; set; }
    public string Resources { get; set; }
    public double CacheSize { get; set; }
}