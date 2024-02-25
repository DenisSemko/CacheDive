namespace IdentityServer.API.Common;

public class AuthenticationResult
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public Guid UserId { get; set; }
    public bool Success { get; set; }
    public IEnumerable<string> Errors { get; set; }
}