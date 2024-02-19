namespace EventBus.Messages.Events.Request;

public class GetRedisExecutionRequest : IntegrationBaseEvent
{
    public string Command { get; set; }
}