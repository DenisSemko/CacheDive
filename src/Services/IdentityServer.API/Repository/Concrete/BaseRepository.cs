namespace IdentityServer.API.Repository.Concrete;

public class BaseRepository<T> : IBaseRepository<T> where T : class
{
    protected readonly ApplicationContext ApplicationContext;

    public BaseRepository(ApplicationContext applicationContext)
    {
        ApplicationContext = applicationContext ?? throw new ArgumentNullException(nameof(applicationContext));
    }

    public async Task<IReadOnlyList<T>> GetAllAsync() => await ApplicationContext.Set<T>().ToListAsync();

    public async Task<IReadOnlyList<T>> GetAllAsync(Expression<Func<T, bool>> predicate) =>
        await ApplicationContext.Set<T>().Where(predicate).ToListAsync();
    
    public async Task<T> GetAsync(Expression<Func<T, bool>> predicate) =>
        await ApplicationContext.Set<T>().Where(predicate).FirstOrDefaultAsync();
    
    public async Task<T> GetByIdAsync(Guid id) => await ApplicationContext.Set<T>().FindAsync(id);

    public async Task<T> InsertOneAsync(T entity)
    { 
        await ApplicationContext.Set<T>().AddAsync(entity);
        await ApplicationContext.SaveChangesAsync();
        return entity;
    }

    public async Task UpdateAsync(T entity)
    {
        ApplicationContext.Set<T>().Update(entity);
        await ApplicationContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(T entity)
    {
        ApplicationContext.Set<T>().Remove(entity);
        await ApplicationContext.SaveChangesAsync();
    }
}