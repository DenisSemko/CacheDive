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
    [ProducesResponseType(typeof(string[]), (int)HttpStatusCode.OK)]
    public ActionResult<string[]> Get()
    {
        string[] baskets = _unitOfWork.Baskets.GetAllKeys();
    
        return Ok(baskets);
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Basket), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<Basket>> GetById(Guid id)
    {
        Basket? basket = await _unitOfWork.Baskets.GetByKeyAsync($"{nameof(Basket)}:{id}");

        return Ok(basket);
    }
    
    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    public async Task<ActionResult<Order>> Post([FromBody] Basket entity)
    {
        Basket result = await _unitOfWork.Baskets.SetAsync($"{nameof(Basket)}:{entity.Id}", entity);
        
        return CreatedAtAction(nameof(Post), result);
    }
    
    
    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult> Delete(Guid id)
    {
        Basket? basket = await _unitOfWork.Baskets.GetByKeyAsync($"{nameof(Basket)}:{id}");
        
        if (basket is not null)
        {
            await _unitOfWork.Baskets.DeleteAsync($"{nameof(Basket)}:{basket.Id}");
        }
        
        return NoContent();
    }
    
    #endregion
}