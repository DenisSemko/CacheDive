namespace IdentityServer.API.Entities;

public sealed class ApplicationUser : IdentityUser<Guid>
{
    public string Name { get; set; }
    public DateTime RegistrationDate { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; }
}