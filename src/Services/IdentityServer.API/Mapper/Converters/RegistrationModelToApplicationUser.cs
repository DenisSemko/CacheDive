namespace IdentityServer.API.Mapper.Converters;

public class RegistrationModelToApplicationUser : ITypeConverter<RegistrationModel, ApplicationUser>
{
    public ApplicationUser Convert(RegistrationModel source, ApplicationUser destination, ResolutionContext context)
    {
        return new ApplicationUser()
        {
            Name = source.Name,
            UserName = source.Username,
            Email = source.Email,
            PasswordHash = source.PasswordHash,
            RegistrationDate = DateTime.Now
        };
    }
}