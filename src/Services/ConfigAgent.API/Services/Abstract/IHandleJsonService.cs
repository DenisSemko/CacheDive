namespace ConfigAgent.API.Services.Abstract;

public interface IHandleJsonService
{
    Task HandleJson(string jsonData, CancellationToken cancellationToken = default);
}