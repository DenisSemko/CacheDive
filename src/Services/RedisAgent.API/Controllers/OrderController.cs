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
    [ProducesResponseType(typeof(string[]), (int)HttpStatusCode.OK)]
    public ActionResult<string[]> Get()
    {
        string[] orders = _unitOfWork.Orders.GetAllKeys();

        return Ok(orders);
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Order), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<Order>> GetById(Guid id)
    {
        Order? order = await _unitOfWork.Orders.GetByKeyAsync($"{nameof(Order)}:{id}");

        return Ok(order);
    }
    
    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    public async Task<ActionResult<Order>> Post([FromBody] Order entity)
    {
        Order result = await _unitOfWork.Orders.SetAsync($"{nameof(Order)}:{entity.Id}", entity);
        
        return CreatedAtAction(nameof(Post), result);
    }
    
    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult> Delete(Guid id)
    {
        Order? order = await _unitOfWork.Orders.GetByKeyAsync($"{nameof(Order)}:{id}");

        if (order is not null)
        {
            await _unitOfWork.Orders.DeleteAsync($"{nameof(Order)}:{order.Id}");
        }
        
        return NoContent();
    }
    
    #endregion
}