namespace IdentityServer.API.Mapper;

public class ApplicationUserProfile : Profile
{
    public ApplicationUserProfile()
    {
        CreateMap<RegistrationModel, ApplicationUser>()
            .ConvertUsing(new RegistrationModelToApplicationUser());
        CreateMap<IReadOnlyCollection<ApplicationUser>, List<ApplicationUserDto>>()
            .ConvertUsing(new CollectionApplicationUserToCollectionApplicationUserDto());
    }
}