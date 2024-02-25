namespace IdentityServer.API.Services.Concrete;

public class AuthenticationResultService : IAuthenticationResultService
{
    private readonly IAccessTokenService _accessTokenService;
    private readonly IRefreshTokenService _refreshTokenService;
    
    public AuthenticationResultService(IAccessTokenService accessTokenService, IRefreshTokenService refreshTokenService)
    {
        _accessTokenService = accessTokenService ?? throw new ArgumentNullException(nameof(accessTokenService));
        _refreshTokenService = refreshTokenService ?? throw new ArgumentNullException(nameof(refreshTokenService));
    }

    public async Task<AuthenticationResult> GenerateAuthenticationResult(ApplicationUser user)
    {
        return new AuthenticationResult
        {
            Success = true,
            AccessToken = await _accessTokenService.Generate(user),
            RefreshToken = await _refreshTokenService.Generate(user),
            UserId = user.Id
        };
    }
}