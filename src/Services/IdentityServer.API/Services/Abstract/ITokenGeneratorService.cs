namespace IdentityServer.API.Services.Abstract;

public interface ITokenGeneratorService
{
    string GenerateToken(Claim[] claims, DateTime expireDate, IList<string>? userRoles = null);
}