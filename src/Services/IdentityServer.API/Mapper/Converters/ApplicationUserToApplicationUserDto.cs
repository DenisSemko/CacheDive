namespace IdentityServer.API.Mapper.Converters;

public class ApplicationUserToApplicationUserDto : ITypeConverter<ApplicationUser, ApplicationUserDto>
{
    public ApplicationUserDto Convert(ApplicationUser source, ApplicationUserDto destination, ResolutionContext context)
    {
        return new ApplicationUserDto(source.Id, source.Name, source.Email, source.UserName, source.RegistrationDate);
    }
}