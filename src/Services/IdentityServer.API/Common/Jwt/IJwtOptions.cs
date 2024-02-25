namespace IdentityServer.API.Common.Jwt;

public interface IJwtOptions
{
    string Audience { get; init; }
    string Issuer { get; init; }
    string Secret { get; init; }
}