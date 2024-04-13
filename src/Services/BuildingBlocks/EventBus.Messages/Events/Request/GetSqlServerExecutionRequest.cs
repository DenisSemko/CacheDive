namespace EventBus.Messages.Events.Request;

public class GetSqlServerExecutionRequest : IntegrationBaseEvent
{
    public ExperimentType ExperimentType { get; set; }
    public int QueryExecutionNumber { get; set; }
    public bool IsCacheCleaned { get; set; }
}