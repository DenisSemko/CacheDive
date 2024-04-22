namespace MongoAgent.API.Repository.Concrete;

public class BaseRepository<T> : IBaseRepository<T> where T : class
{
    private readonly IMongoCollection<T> _collection;

    public BaseRepository(IDbSettings settings)
    {
        var database = new MongoClient(settings.ConnectionString).GetDatabase(settings.DatabaseName);
        _collection = database.GetCollection<T>(GetCollectionName(typeof(T)));
    }
    
    public async Task<IReadOnlyList<T>> GetAllAsync() => await _collection.Find(_ => true).ToListAsync();

    public async Task<IReadOnlyList<T>> GetAllAsync(Expression<Func<T, bool>> predicate) =>
        await _collection.Find(predicate).ToListAsync();
    
    public async Task<T> GetAsync(Expression<Func<T, bool>> predicate) =>
        await _collection.Find(predicate).SingleOrDefaultAsync();

    public async Task<T> GetByIdAsync(Guid id)
    {
        var filter = Builders<T>.Filter.Eq("_id", id.ToString());
        return await _collection.Find(filter).SingleOrDefaultAsync();
    }

    public async Task<T> InsertOneAsync(T entity)
    {
        await _collection.InsertOneAsync(entity);
        return entity;
    }

    public async Task UpdateAsync(Guid id, T entity)
    {
        var filter = Builders<T>.Filter.Eq("_id", id);
        await _collection.FindOneAndReplaceAsync(filter, entity);
    }

    public async Task DeleteAsync(Guid id)
    {
        var filter = Builders<T>.Filter.Eq("_id", id);
        await _collection.DeleteOneAsync(filter);
    }
    
    private string? GetCollectionName(Type documentType)
    {
        return ((BsonCollectionAttribute)documentType.GetCustomAttributes(
                typeof(BsonCollectionAttribute),
                true)
            .FirstOrDefault()!)?.CollectionName;
    }
}