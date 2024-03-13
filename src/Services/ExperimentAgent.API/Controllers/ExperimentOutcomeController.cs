namespace ExperimentAgent.API.Controllers;

/// <summary>
/// Controller for ExperimentOutcome entity operations.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ExperimentOutcomeController : Controller
{
    #region PrivateFields
    
    private readonly IUnitOfWork _unitOfWork;
    
    #endregion
    
    #region ctor
    
    public ExperimentOutcomeController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }
    
    #endregion
    
    #region ControllerMethods

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<ExperimentOutcome>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IReadOnlyList<ExperimentOutcome>>> GetAll()
    {
        IReadOnlyList<ExperimentOutcome> experimentOutcomes = await _unitOfWork.ExperimentOutcomes.GetAllAsync();

        return Ok(experimentOutcomes);
    }
    
    [HttpGet("query/{query}")]
    [ProducesResponseType(typeof(IReadOnlyList<ExperimentOutcome>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IReadOnlyList<ExperimentOutcome>>> GetByQuery(string query)
    {
        IReadOnlyList<ExperimentOutcome> outcomes = await _unitOfWork.ExperimentOutcomes.GetAllAsync(outcome => outcome.Query == query);

        return Ok(outcomes);
    }
    
    [HttpGet("databaseType/{databaseType}")]
    [ProducesResponseType(typeof(IReadOnlyList<ExperimentOutcome>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IReadOnlyList<ExperimentOutcome>>> GetByDatabaseType(DatabaseType databaseType)
    {
        IReadOnlyList<ExperimentOutcome> outcomes = await _unitOfWork.ExperimentOutcomes.GetAllAsync(outcome => outcome.DatabaseType == databaseType);

        return Ok(outcomes);
    }
    
    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult> Delete(Guid id)
    {
        ExperimentOutcome outcome = await _unitOfWork.ExperimentOutcomes.GetByIdAsync(id);
        await _unitOfWork.ExperimentOutcomes.DeleteAsync(outcome);
        return NoContent();
    }
    
    #endregion
}