namespace ExperimentAgent.API.Repository.Abstract;

public interface IUnitOfWork
{
    IBaseRepository<ExperimentOutcome> ExperimentOutcomes { get; }
}