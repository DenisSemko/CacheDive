namespace IdentityServer.API.Common.Response;

public record AuthSuccessResponse(string AccessToken, string RefreshToken, Guid UserId);