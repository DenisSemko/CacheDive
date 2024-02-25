namespace IdentityServer.API.Common.Jwt;

public class JwtOptions : IJwtOptions
{
    public string Audience { get; init; }
    public string Issuer { get; init; }
    public string Secret { get; init; }
}