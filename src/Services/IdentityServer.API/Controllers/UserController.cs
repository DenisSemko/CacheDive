using System.IdentityModel.Tokens.Jwt;

namespace IdentityServer.API.Controllers;

/// <summary>
/// Controller for User operations.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class UserController : Controller
{
    #region PrivateFields
    
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    
    #endregion
    
    #region ctor

    public UserController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }
    
    #endregion
    
    #region ControllerMethods
    
    /// <summary>
    /// Gets Users.
    /// </summary>
    /// <returns>
    /// Returns a List of Users.
    /// </returns>
    [HttpGet]
    [ProducesResponseType(typeof(List<ApplicationUserDto>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<List<ApplicationUserDto>>> GetAll()
    {
        IReadOnlyCollection<ApplicationUser> users = await _unitOfWork.ApplicationUsers.GetAllAsync();

        return Ok(_mapper.Map<List<ApplicationUserDto>>(users));
    }
    
    /// <summary>
    /// Gets a User by Id.
    /// </summary>
    /// <param name="id">
    /// User's ID to get details.
    /// </param>
    /// <returns>
    /// Returns User details by Id.
    /// </returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApplicationUserDto), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<ApplicationUserDto>> GetById(Guid id)
    {
        ApplicationUser user = await _unitOfWork.ApplicationUsers.GetByIdAsync(id);

        return Ok(_mapper.Map<ApplicationUserDto>(user));
    }
    
    /// <summary>
    /// Gets a current User.
    /// </summary>
    /// <returns>
    /// Returns User details.
    /// </returns>
    [HttpGet("current")]
    [ProducesResponseType(typeof(ApplicationUserDto), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<ApplicationUserDto>> GetCurrentUser()
    {
        string? accessToken = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        string userId = string.Empty;

        if (accessToken is not null)
        {
            JwtSecurityTokenHandler tokenHandler = new();

            JwtSecurityToken token = tokenHandler.ReadToken(accessToken) as JwtSecurityToken;

            if (token is not null)
            {
                userId = token.Claims.FirstOrDefault(claim => claim.Type == "id").Value;
            }
        }
        
        ApplicationUser user = await _unitOfWork.ApplicationUsers.GetByIdAsync(Guid.Parse(userId));

        return Ok(_mapper.Map<ApplicationUserDto>(user));
    }
    
    #endregion
}