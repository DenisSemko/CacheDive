namespace IdentityServer.API.Services.Abstract;

public interface IAuthService
{
    Task<AuthenticationResult> RegisterAsync(RegistrationModel registrationModel);
    Task<AuthenticationResult> LoginAsync(LoginModel loginModel);
}