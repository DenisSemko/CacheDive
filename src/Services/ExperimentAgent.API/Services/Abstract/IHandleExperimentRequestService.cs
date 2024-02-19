namespace ExperimentAgent.API.Services.Abstract;

public interface IHandleExperimentRequestService
{
    Task<ExperimentResult> HandleRequest(ExperimentRequest request, CancellationToken cancellationToken = default);
}