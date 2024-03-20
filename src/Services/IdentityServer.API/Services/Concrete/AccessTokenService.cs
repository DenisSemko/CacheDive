namespace IdentityServer.API.Services.Concrete;

public class AccessTokenService : IAccessTokenService
{
    private readonly ITokenGeneratorService _tokenGeneratorService;
    private readonly UserManager<ApplicationUser> _userManager;

    public AccessTokenService(ITokenGeneratorService tokenGeneratorService, UserManager<ApplicationUser> userManager)
    {
        _tokenGeneratorService = tokenGeneratorService ?? throw new ArgumentNullException(nameof(tokenGeneratorService));
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
    }

    public async Task<string> Generate(ApplicationUser user)
    {
        IList<string?> userRoles = await _userManager.GetRolesAsync(user);
        Claim[] claims = {
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(ClaimTypes.Name, user.UserName),
            new("id", user.Id.ToString())
        };

        return _tokenGeneratorService.GenerateToken(claims, DateTime.UtcNow.AddMinutes(300), userRoles);
    }
}