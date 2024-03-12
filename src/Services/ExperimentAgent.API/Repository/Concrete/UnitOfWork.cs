namespace ExperimentAgent.API.Repository.Concrete;

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

    private IBaseRepository<ExperimentOutcome> _experimentOutcomeRepository;
    public IBaseRepository<ExperimentOutcome> ExperimentOutcomes => _experimentOutcomeRepository ?? new BaseRepository<ExperimentOutcome>(_applicationContext);

    #endregion
}