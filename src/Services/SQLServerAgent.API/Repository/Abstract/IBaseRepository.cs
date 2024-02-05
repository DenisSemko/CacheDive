namespace SQLServerAgent.API.Repository.Abstract;

public interface IBaseRepository <T> where T : class
{
    #region Methods
    
    Task<IReadOnlyList<T>> GetAllAsync();
    Task<IReadOnlyList<T>> GetAllAsync(Expression<Func<T, bool>> predicate);
    Task<T> GetAsync(Expression<Func<T, bool>> predicate);
    Task<T> GetByIdAsync(Guid id);
    Task<T> InsertOneAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
    
    #endregion
}