namespace ExperimentAgent.API.Repository.Concrete;

public class BaseRepository<T> : IBaseRepository<T> where T : class
{
    protected readonly ApplicationContext _dbContext;

    public BaseRepository(ApplicationContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }
    
    public async Task<IReadOnlyList<T>> GetAllAsync() => await _dbContext.Set<T>().ToListAsync();

    public async Task<IReadOnlyList<T>> GetAllAsync(Expression<Func<T, bool>> predicate) =>
        await _dbContext.Set<T>().Where(predicate).ToListAsync();
    
    public async Task<T> GetAsync(Expression<Func<T, bool>> predicate) =>
        await _dbContext.Set<T>().Where(predicate).FirstOrDefaultAsync();
    
    public async Task<T> GetByIdAsync(Guid id) => await _dbContext.Set<T>().FindAsync(id);

    public async Task<T> InsertOneAsync(T entity)
    { 
        await _dbContext.Set<T>().AddAsync(entity);
        await _dbContext.SaveChangesAsync();
        return entity;
    }

    public async Task UpdateAsync(T entity)
    {
        _dbContext.Set<T>().Update(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(T entity)
    {
        _dbContext.Set<T>().Remove(entity);
        await _dbContext.SaveChangesAsync();
    }
}