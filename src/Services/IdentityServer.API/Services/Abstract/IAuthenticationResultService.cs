namespace IdentityServer.API.Services.Abstract;

public interface IAuthenticationResultService
{
    Task<AuthenticationResult> GenerateAuthenticationResult(ApplicationUser user);
}