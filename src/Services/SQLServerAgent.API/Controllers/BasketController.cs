namespace SQLServerAgent.API.Controllers;

/// <summary>
/// Controller for Basket entity operations.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class BasketController : Controller
{ 
    #region PrivateFields
    
    private readonly IUnitOfWork _unitOfWork;
    
    #endregion
    
    #region ctor
    
    public BasketController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }
    
    #endregion
    
    #region ControllerMethods

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<Basket>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IReadOnlyList<Basket>>> Get()
    {
        IReadOnlyList<Basket> baskets = await _unitOfWork.Baskets.GetAllAsync();

        return Ok(baskets);
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Basket), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<Basket>> GetById(Guid id)
    {
        Basket basket = await _unitOfWork.Baskets.GetByIdAsync(id);

        return Ok(basket);
    }
    
    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    public async Task<ActionResult<Order>> Post([FromBody] Basket entity)
    {
        Basket result = await _unitOfWork.Baskets.InsertOneAsync(entity);
        
        return CreatedAtAction(nameof(Post), result);
    }
    
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult> Update([FromBody] Basket entity)
    {
        await _unitOfWork.Baskets.UpdateAsync(entity);
        return NoContent();
    }
    
    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult> Delete(Guid id)
    {
        Basket basket = await _unitOfWork.Baskets.GetByIdAsync(id);
        await _unitOfWork.Baskets.DeleteAsync(basket);
        return NoContent();
    }
    
    #endregion
}