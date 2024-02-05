namespace ConfigAgent.API.Controllers;

/// <summary>
/// Controller for General operations.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ConfigAgentController : Controller
{
    #region PrivateFields
    
    private readonly IHandleJsonService _handleJsonService;
    
    #endregion
    
    #region ctor
    
    public ConfigAgentController(IHandleJsonService handleJsonService)
    {
        _handleJsonService = handleJsonService ?? throw new ArgumentNullException(nameof(handleJsonService));
    }
    
    #endregion
    
    #region ControllerMethods
    
    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<ActionResult> PostAsync(IFormFile jsonFile)
    {
        try
        {
            using (var streamReader = new StreamReader(jsonFile.OpenReadStream()))
            {
                string fileContent = streamReader.ReadToEnd();
                
                await _handleJsonService.HandleJson(fileContent);
        
                return Ok();
            }
        }
        catch (Exception ex)
        {
            throw;
        }
       
    }
    
    #endregion
}