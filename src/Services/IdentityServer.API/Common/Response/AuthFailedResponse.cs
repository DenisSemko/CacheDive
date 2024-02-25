namespace IdentityServer.API.Common.Response;

public record AuthFailedResponse(IEnumerable<string> Errors);