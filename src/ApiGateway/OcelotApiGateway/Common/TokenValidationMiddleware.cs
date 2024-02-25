namespace OcelotApiGateway.Common;

public class TokenValidationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IConfiguration _configuration;
    
    public TokenValidationMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        _next = next;
        _configuration = configuration;
    }

    public async Task Invoke(HttpContext context)
    {
        if (context.Request.Path.Value.Contains(Constants.AuthRequestPaths.LoginPath) ||
            context.Request.Path.Value.Contains(Constants.AuthRequestPaths.RegistrationPath))
        {
            await _next(context);
        }
        else
        {
            context.Request.Headers.Add("Authorization", context.Request.Cookies["Authorization"]);
            string? accessToken = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            string? refreshToken = context.Request.Cookies["Refresh"];

            if (accessToken is not null)
            {
                try
                {
                    JwtSecurityTokenHandler tokenHandler = new();
                    tokenHandler.ValidateToken(accessToken, CreateTokenValidationParams(), out var validatedToken);
                    context.Items["ValidatedToken"] = validatedToken;
                }
                catch (SecurityTokenExpiredException)
                {
                    try
                    {
                        JwtSecurityTokenHandler tokenHandler = new();
                        HttpClient httpClient = new ();
                        tokenHandler.ValidateToken(refreshToken, CreateTokenValidationParams(), out var validatedToken);
                        
                        HttpResponseMessage response = await httpClient.GetAsync(string.Format(Constants.AuthRequestPaths.RefreshPath, accessToken));
                        string content = await response.Content.ReadAsStringAsync();
                        TokenModel model = JsonConvert.DeserializeObject<TokenModel>(content);

                        context.Request.Headers["Authorization"] = model.AccessToken;
                        context.Response.Cookies.Append("Authorization", model.AccessToken, new CookieOptions
                        {
                           HttpOnly = true
                        });
                        context.Response.Cookies.Append("Refresh", model.RefreshToken, new CookieOptions
                        {
                           HttpOnly = true
                        });

                    }
                    catch (Exception)
                    {
                        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                        return;
                    }
                }
                catch (Exception)
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return;
                }
            }
            else
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }
            await _next(context); 
        }
    }

    private TokenValidationParameters CreateTokenValidationParams()
    {
        byte[] key = Encoding.ASCII.GetBytes(_configuration["JWT:Secret"]);
        TokenValidationParameters tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = true,
            ValidIssuer = _configuration["JWT:Issuer"],
            ValidateAudience = true,
            ValidAudience = _configuration["JWT:Audience"],
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };

        return tokenValidationParameters;
    } 
}