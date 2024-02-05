namespace ConfigAgent.API.Services.Concrete;

public class HandleJsonService : IHandleJsonService
{
    private readonly IValidateJsonService _validateJsonService;
    private readonly IPublishEndpoint _publishEndpoint;

    public HandleJsonService(IValidateJsonService validateJsonService, IPublishEndpoint publishEndpoint)
    {
        _validateJsonService = validateJsonService ?? throw new ArgumentNullException(nameof(validateJsonService));
        _publishEndpoint = publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));
    }
    
    public async Task HandleJson(string jsonData, CancellationToken cancellationToken = default)
    {
        _validateJsonService.IsValidJson(jsonData);
        
        TransferJsonEvent transferJsonEvent = new() { DatabaseData = jsonData };

        await _publishEndpoint.Publish<TransferJsonEvent>(transferJsonEvent, cancellationToken);
    }
}