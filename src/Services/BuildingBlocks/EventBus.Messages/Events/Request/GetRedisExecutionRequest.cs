namespace EventBus.Messages.Events.Request;

public class GetRedisExecutionRequest : IntegrationBaseEvent
{
    public ExperimentType ExperimentType { get; set; }
    public int QueryExecutionNumber { get; set; }
}