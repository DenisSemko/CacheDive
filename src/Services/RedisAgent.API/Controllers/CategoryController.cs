namespace SQLServerAgent.API.Controllers;

/// <summary>
/// Controller for Category entity operations.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class CategoryController : Controller
{
    #region PrivateFields
    
    private readonly IUnitOfWork _unitOfWork;
    
    #endregion
    
    #region ctor
    
    public CategoryController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }
    
    #endregion
    
    #region ControllerMethods

    [HttpGet]
    [ProducesResponseType(typeof(string[]), (int)HttpStatusCode.OK)]
    public ActionResult<string[]> Get()
    {
        string[] categories = _unitOfWork.Categories.GetAllKeys();
    
        return Ok(categories);
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Category), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<Category>> GetById(Guid id)
    {
        Category? category = await _unitOfWork.Categories.GetByKeyAsync($"{nameof(Category)}:{id}");

        return Ok(category);
    }
    
    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    public async Task<ActionResult<Category>> Post([FromBody] Category entity)
    {
        Category result = await _unitOfWork.Categories.SetAsync($"{nameof(Category)}:{entity.Id}", entity);
        
        return CreatedAtAction(nameof(Post), result);
    }
    
    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult> Delete(Guid id)
    {
        Category? category = await _unitOfWork.Categories.GetByKeyAsync($"{nameof(Category)}:{id}");

        if (category is not null)
        {
            await _unitOfWork.Categories.DeleteAsync($"{nameof(Category)}:{category.Id}");
        }
        return NoContent();
    }
    
    #endregion
}