using System.IdentityModel.Tokens.Jwt;

namespace IdentityServer.API.Services.Concrete;

public class TokenGeneratorService : ITokenGeneratorService
{
    private readonly IJwtOptions _jwtOptions;

    public TokenGeneratorService(IJwtOptions jwtOptions)
    {
        _jwtOptions = jwtOptions ?? throw new ArgumentNullException(nameof(jwtOptions));
    }

    public string GenerateToken(Claim[] claims, DateTime expireDate, IList<string>? userRoles = null)
    {
        JwtSecurityTokenHandler tokenHandler = new ();
        byte[] key = Encoding.ASCII.GetBytes(_jwtOptions.Secret);

        SecurityTokenDescriptor securityTokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = expireDate,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Issuer = _jwtOptions.Issuer,
            Audience = _jwtOptions.Audience
        };

        if (userRoles is not null)
        {
            foreach (var userRole in userRoles)
            {
                securityTokenDescriptor.Subject.AddClaim(new Claim(ClaimTypes.Role, userRole));
            }
        }
        
        SecurityToken token = tokenHandler.CreateToken(securityTokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}