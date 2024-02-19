namespace ExperimentAgent.API.Controllers;

/// <summary>
/// Controller for Request operations.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ExperimentRequestController : Controller
{
    #region PrivateFields
    
    private readonly IHandleExperimentRequestService _experimentRequestService;
    private readonly ExperimentRequestValidator _experimentRequestValidator;
    
    #endregion
    
    #region ctor
    
    public ExperimentRequestController(IHandleExperimentRequestService experimentRequestService)
    {
        _experimentRequestService = experimentRequestService ?? throw new ArgumentNullException(nameof(experimentRequestService));
        _experimentRequestValidator = new ExperimentRequestValidator();
    }
    
    #endregion
    
    #region ControllerMethods
    
    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<ActionResult<ExperimentRequest>> PostAsync([FromBody] ExperimentRequest experimentRequest)
    {
        try
        {

            await _experimentRequestValidator.ValidateAsync(experimentRequest, options => options.ThrowOnFailures());
            
            ExperimentResult experimentResult = await _experimentRequestService.HandleRequest(experimentRequest);
            
            return Ok(experimentResult);
        }
        catch (Exception ex)
        {
            throw;
        }
    }
    
    #endregion
}