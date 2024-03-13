namespace IdentityServer.API.Mapper.Converters;

public class CollectionApplicationUserToCollectionApplicationUserDto : ITypeConverter<IReadOnlyCollection<ApplicationUser>, List<ApplicationUserDto>>
{
    public List<ApplicationUserDto> Convert(IReadOnlyCollection<ApplicationUser> source, List<ApplicationUserDto> destination, ResolutionContext context)
    {
        return source.Select(user => new ApplicationUserDto(user.Id, user.Name, user.Email, user.UserName, user.RegistrationDate)).ToList();
    }
}