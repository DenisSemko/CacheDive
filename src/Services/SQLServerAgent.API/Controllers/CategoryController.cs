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
    [ProducesResponseType(typeof(IReadOnlyList<Category>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IReadOnlyList<Category>>> Get()
    {
        IReadOnlyList<Category> categories = await _unitOfWork.Categories.GetAllAsync();

        return Ok(categories);
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Category), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<Category>> GetById(Guid id)
    {
        Category category = await _unitOfWork.Categories.GetByIdAsync(id);

        return Ok(category);
    }
    
    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    public async Task<ActionResult<Category>> Post([FromBody] Category entity)
    {
        Category result = await _unitOfWork.Categories.InsertOneAsync(entity);
        
        return CreatedAtAction(nameof(Post), result);
    }
    
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult> Update([FromBody] Category entity)
    {
        await _unitOfWork.Categories.UpdateAsync(entity);
        return NoContent();
    }
    
    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult> Delete(Guid id)
    {
        Category category = await _unitOfWork.Categories.GetByIdAsync(id);
        await _unitOfWork.Categories.DeleteAsync(category);
        return NoContent();
    }
    
    #endregion
}