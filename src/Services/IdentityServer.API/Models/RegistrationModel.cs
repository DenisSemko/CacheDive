namespace IdentityServer.API.Models;

public class RegistrationModel
{
    public string Name { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
}