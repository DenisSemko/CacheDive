namespace IdentityServer.API.Repository.Concrete;

public class UnitOfWork : IUnitOfWork
{
    #region Private fields
    private readonly ApplicationContext _applicationContext;
    #endregion

    #region ctor
    public UnitOfWork(ApplicationContext applicationContext)
    {
        _applicationContext = applicationContext;
    }
    #endregion
    
    #region Repositories
    
    private IBaseRepository<ApplicationUser> _userRepository;
    public IBaseRepository<ApplicationUser> ApplicationUsers => _userRepository ?? new BaseRepository<ApplicationUser>(_applicationContext);
    
    #endregion
}