namespace EventBus.Messages.Events;

public class JsonDataPublishedEvent : IntegrationBaseEvent
{
    public string DatabaseData { get; set; }
}