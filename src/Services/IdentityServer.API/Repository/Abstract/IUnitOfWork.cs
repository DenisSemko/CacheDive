namespace IdentityServer.API.Repository.Abstract;

public interface IUnitOfWork
{
    IBaseRepository<ApplicationUser> ApplicationUsers { get; }
}