namespace EventBus.Messages.Events;

public class TransferJsonEvent : IntegrationBaseEvent
{
    public string DatabaseData { get; set; }
}