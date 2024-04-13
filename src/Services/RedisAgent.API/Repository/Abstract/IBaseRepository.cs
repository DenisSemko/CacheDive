namespace RedisAgent.API.Repository.Abstract;

public interface IBaseRepository <T> where T : class
{
    #region Methods

    string[] GetAllKeys();
    Task<T?> GetByKeyAsync(string key);
    Task<T> SetAsync(string key, T entity);
    Task DeleteAsync(string key);
    
    #endregion
}