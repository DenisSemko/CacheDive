namespace EventBus.Messages.Events.Request;

public class GetMemcachedExecutionRequest : IntegrationBaseEvent
{
    public string Command { get; set; }
}