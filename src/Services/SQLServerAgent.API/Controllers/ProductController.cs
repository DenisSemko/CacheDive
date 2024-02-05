namespace SQLServerAgent.API.Controllers;

/// <summary>
/// Controller for Product entity operations.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ProductController : Controller
{
    #region PrivateFields
    
    private readonly IUnitOfWork _unitOfWork;
    
    #endregion
    
    #region ctor
    
    public ProductController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }
    
    #endregion
    
    #region ControllerMethods

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<Product>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IReadOnlyList<Product>>> Get()
    {
        IReadOnlyList<Product> products = await _unitOfWork.Products.GetAllAsync();

        return Ok(products);
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<Product>> GetById(Guid id)
    {
        Product products = await _unitOfWork.Products.GetByIdAsync(id);

        return Ok(products);
    }
    
    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    public async Task<ActionResult<Product>> Post([FromBody] Product entity)
    {
        Product result = await _unitOfWork.Products.InsertOneAsync(entity);
        
        return CreatedAtAction(nameof(Post), result);
    }
    
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult> Update([FromBody] Product entity)
    {
        await _unitOfWork.Products.UpdateAsync(entity);
        return NoContent();
    }
    
    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult> Delete(Guid id)
    {
        Product product = await _unitOfWork.Products.GetByIdAsync(id);
        await _unitOfWork.Products.DeleteAsync(product);
        return NoContent();
    }
    
    #endregion
}