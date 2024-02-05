namespace SQLServerAgent.API.Controllers;

/// <summary>
/// Controller for Order entity operations.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class OrderController : Controller
{
    #region PrivateFields
    
    private readonly IUnitOfWork _unitOfWork;
    
    #endregion
    
    #region ctor
    
    public OrderController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }
    
    #endregion
    
    #region ControllerMethods

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<Order>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IReadOnlyList<Order>>> Get()
    {
        IReadOnlyList<Order> orders = await _unitOfWork.Orders.GetAllAsync();

        return Ok(orders);
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Order), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<Order>> GetById(Guid id)
    {
        Order order = await _unitOfWork.Orders.GetByIdAsync(id);

        return Ok(order);
    }
    
    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    public async Task<ActionResult<Order>> Post([FromBody] Order entity)
    {
        Order result = await _unitOfWork.Orders.InsertOneAsync(entity);
        
        return CreatedAtAction(nameof(Post), result);
    }
    
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult> Update([FromBody] Order entity)
    {
        await _unitOfWork.Orders.UpdateAsync(entity);
        return NoContent();
    }
    
    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult> Delete(Guid id)
    {
        Order order = await _unitOfWork.Orders.GetByIdAsync(id);
        await _unitOfWork.Orders.DeleteAsync(order);
        return NoContent();
    }
    
    #endregion
}