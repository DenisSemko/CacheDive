using UserEntity = RedisAgent.API.Entities.User;

namespace RedisAgent.API.Controllers;

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
    [ProducesResponseType(typeof(string[]), (int)HttpStatusCode.OK)]
    public ActionResult<string[]> Get()
    {
        string[] users = _unitOfWork.Users.GetAllKeys();

        return Ok(users);
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(User), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<User>> GetById(Guid id)
    {
        User? user = await _unitOfWork.Users.GetByKeyAsync($"{nameof(UserEntity)}:{id}");

        return Ok(user);
    }
    
    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    public async Task<ActionResult<User>> Post([FromBody] User entity)
    {
        User result = await _unitOfWork.Users.SetAsync($"{nameof(UserEntity)}:{entity.Id}", entity);
        
        return CreatedAtAction(nameof(Post), result);
    }
    
    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult> Delete(Guid id)
    {
        User? user = await _unitOfWork.Users.GetByKeyAsync($"{nameof(UserEntity)}:{id}");

        if (user is not null)
        {
            await _unitOfWork.Users.DeleteAsync($"{nameof(UserEntity)}:{user.Id}");
        }
        
        return NoContent();
    }
    
    #endregion
}