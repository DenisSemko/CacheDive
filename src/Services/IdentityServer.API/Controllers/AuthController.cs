using System.IdentityModel.Tokens.Jwt;

namespace IdentityServer.API.Controllers;

/// <summary>
/// Controller for Identity operations.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
   #region PrivateFields
   
   private readonly IAuthService _authService;
   private readonly UserManager<ApplicationUser> _userManager;
   private readonly IAuthenticationResultService _authenticationResultService;
   
   #endregion
   
   #region ctor
   
   public AuthController(IAuthService authService, UserManager<ApplicationUser> userManager, IAuthenticationResultService authenticationResultService)
   {
      _authService = authService ?? throw new ArgumentNullException(nameof(authService));
      _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
      _authenticationResultService = authenticationResultService ?? throw new ArgumentNullException(nameof(authenticationResultService));
   }
   
   #endregion

   #region ControllerMethods
   
   /// <summary>
   /// Registers a new User.
   /// </summary>
   /// <param name="registrationModel">
   /// Registration Model.
   /// </param>
   /// <returns>
   /// Returns a new User.
   /// </returns>
   [HttpPost]
   [Route("registration")]
   [ProducesResponseType(typeof(RegistrationModel), (int)HttpStatusCode.Created)]
   public async Task<IActionResult> Register([FromBody] RegistrationModel registrationModel)
   {
      AuthenticationResult authResponse = await _authService.RegisterAsync(registrationModel);
      
      if (!authResponse.Success)
      {
         return BadRequest(new AuthFailedResponse(authResponse.Errors));
      }

      return Ok(new AuthSuccessResponse(authResponse.AccessToken, authResponse.RefreshToken, authResponse.UserId));
   }
   
   /// <summary>
   /// Logins user to the system.
   /// </summary>
   /// <param name="loginModel">
   /// Login Model.
   /// </param>
   /// <returns>
   /// Returns login's model.
   /// </returns>
   [HttpPost]
   [Route("login")]
   [ProducesResponseType(typeof(LoginModel), (int)HttpStatusCode.OK)]
   public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
   {
      AuthenticationResult authResponse = await _authService.LoginAsync(loginModel);
      
      if (!authResponse.Success)
      {
         return BadRequest(new AuthFailedResponse(authResponse.Errors));
      }
      
      Response.Cookies.Append("Authorization", authResponse.AccessToken, new CookieOptions
      {
         HttpOnly = true
      });
      
      Response.Cookies.Append("Refresh", authResponse.RefreshToken, new CookieOptions
      {
         HttpOnly = true
      });

      return Ok(new AuthSuccessResponse(authResponse.AccessToken, authResponse.RefreshToken, authResponse.UserId));
   }
   
   /// <summary>
   /// Logouts user from the system.
   /// </summary>
   /// <param name="username">
   /// User's username.
   /// </param>
   /// <returns>
   /// Returns No Content.
   /// </returns>
   [HttpPost]
   [Route("logout")]
   [ProducesResponseType(typeof(LoginModel), (int)HttpStatusCode.NoContent)]
   public async Task<IActionResult> Logout([FromBody] LogoutModel logoutModel)
   {
      ApplicationUser user = await _userManager.FindByNameAsync(logoutModel.Username);
      
      if (user is not null)
      {
         Response.Cookies.Delete("Authorization");
         Response.Cookies.Delete("Refresh");
      }
      
      return NoContent();
   }
   
   /// <summary>
   /// Refreshes user's tokens.
   /// </summary>
   /// <param name="accessToken">
   /// User's access token.
   /// </param>
   /// <returns>
   /// Returns AuthSuccessResponse model.
   /// </returns>
   [HttpGet("{accessToken}")]
   [ProducesResponseType(typeof(AuthSuccessResponse), (int)HttpStatusCode.OK)]
   public async Task<ActionResult<AuthSuccessResponse>> RefreshTokens(string accessToken)
   {
      JwtSecurityTokenHandler tokenHandler = new ();
      JwtSecurityToken accessTokenInfo = tokenHandler.ReadJwtToken(accessToken);
      Claim? userNameClaim = accessTokenInfo.Claims.FirstOrDefault(claim => claim.Type == "name");
      ApplicationUser existingUser = await _userManager.FindByNameAsync(userNameClaim.Value);
      AuthenticationResult response = await _authenticationResultService.GenerateAuthenticationResult(existingUser);
      
      return Ok(new AuthSuccessResponse(response.AccessToken, response.RefreshToken, Guid.Empty));
   }
   #endregion
}