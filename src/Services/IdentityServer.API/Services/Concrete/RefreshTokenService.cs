namespace IdentityServer.API.Services.Concrete;

public class RefreshTokenService : IRefreshTokenService
{
    private readonly ITokenGeneratorService _tokenGeneratorService;
    private readonly UserManager<ApplicationUser> _userManager;

    public RefreshTokenService(ITokenGeneratorService tokenGeneratorService, UserManager<ApplicationUser> userManager)
    {
        _tokenGeneratorService = tokenGeneratorService ?? throw new ArgumentNullException(nameof(tokenGeneratorService));
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
    }

    public async Task<string> Generate(ApplicationUser user)
    {
        Claim[] claims = {
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new("id", user.Id.ToString())
        };
        string refreshToken = _tokenGeneratorService.GenerateToken(claims, DateTime.UtcNow.AddDays(7));
        
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
        await _userManager.UpdateAsync(user);

        return refreshToken;
    }
}