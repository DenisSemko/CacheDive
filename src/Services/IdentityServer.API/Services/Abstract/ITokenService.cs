namespace IdentityServer.API.Services.Abstract;

public interface ITokenService
{
    Task<string> Generate(ApplicationUser user);
}