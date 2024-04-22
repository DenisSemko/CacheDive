namespace MongoAgent.API.Controllers;

/// <summary>
/// Controller for User entity operations.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class UserController : Controller
{
    #region PrivateFields
    
    private readonly IUnitOfWork _unitOfWork;
    
    #endregion
    
    #region ctor
    
    public UserController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }
    
    #endregion
    
    #region ControllerMethods

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<User>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IReadOnlyList<User>>> Get()
    {
        IReadOnlyList<User> users = await _unitOfWork.Users.GetAllAsync();

        return Ok(users);
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(User), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<User>> GetById(Guid id)
    {
        User user = await _unitOfWork.Users.GetByIdAsync(id);

        return Ok(user);
    }
    
    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    public async Task<ActionResult<User>> Post([FromBody] User entity)
    {
        User result = await _unitOfWork.Users.InsertOneAsync(entity);
        
        return CreatedAtAction(nameof(Post), result);
    }
    
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult> Update([FromBody] User entity)
    {
        await _unitOfWork.Users.UpdateAsync(entity.Id, entity);
        return NoContent();
    }
    
    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult> Delete(Guid id)
    {
        await _unitOfWork.Users.DeleteAsync(id);
        return NoContent();
    }
    
    #endregion
}