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
    [ProducesResponseType(typeof(string[]), (int)HttpStatusCode.OK)]
    public ActionResult<string[]> Get()
    {
        string[] products = _unitOfWork.Products.GetAllKeys();

        return Ok(products);
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<Product>> GetById(Guid id)
    {
        Product? products = await _unitOfWork.Products.GetByKeyAsync($"{nameof(Product)}:{id}");

        return Ok(products);
    }
    
    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    public async Task<ActionResult<Product>> Post([FromBody] Product entity)
    {
        Product result = await _unitOfWork.Products.SetAsync($"{nameof(Product)}:{entity.Id}", entity);
        
        return CreatedAtAction(nameof(Post), result);
    }
    
    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult> Delete(Guid id)
    {
        Product? product = await _unitOfWork.Products.GetByKeyAsync($"{nameof(Product)}:{id}");

        if (product is not null)
        {
            await _unitOfWork.Products.DeleteAsync($"{nameof(Product)}:{product.Id}");
        }
        
        return NoContent();
    }
    
    #endregion
}