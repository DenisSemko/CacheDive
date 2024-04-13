using StackExchange.Redis;

namespace RedisAgent.API.Repository.Concrete;

public class BaseRepository<T> : IBaseRepository<T> where T : class
{
    private readonly IConnectionMultiplexer _connectionMultiplexer;
    private IDatabase _database;

    public BaseRepository(IConnectionMultiplexer connectionMultiplexer)
    {
        _connectionMultiplexer = connectionMultiplexer ?? throw new ArgumentNullException(nameof(connectionMultiplexer));
        _database = _connectionMultiplexer.GetDatabase();
    }

    public string[] GetAllKeys()
    {
        IServer server = _connectionMultiplexer.GetServer(_connectionMultiplexer.GetEndPoints()[0]);
        return server.Keys(pattern: typeof(T).Name + ":" + "*").Select(key => (string)key).ToArray();
    }

    public async Task<T?> GetByKeyAsync(string key)
    {
        HashEntry[] hashEntries = await _database.HashGetAllAsync(key);
        
        return hashEntries.Length > 0 ? Deserialize(hashEntries) : null;
    }

    public async Task<T> SetAsync(string key, T entity)
    {
        HashEntry[] hashEntries = Serialize(entity);
        await _database.HashSetAsync(key, hashEntries);

        return await GetByKeyAsync(key);
    }

    public async Task DeleteAsync(string key)
    {
        await _database.KeyDeleteAsync(key);
    }
    
    private HashEntry[] Serialize(T entity)
    {
        PropertyInfo[] properties = typeof(T).GetProperties();
        List<HashEntry> hashEntries = new ();
        
        foreach (var property in properties)
        {
            object? value = property.GetValue(entity);
            if (value is not null)
            {
                hashEntries.Add(new HashEntry(property.Name, Convert.ToString(value)));
            }
        }
        return hashEntries.ToArray();
    }
    
    private T Deserialize(HashEntry[] hashEntries)
    {
        T entity = Activator.CreateInstance<T>();
        PropertyInfo[] properties = typeof(T).GetProperties();

        foreach (var property in properties)
        {
            HashEntry hashEntry = Array.Find(hashEntries, entry => entry.Name == property.Name);
            
            if (hashEntry.Value.HasValue)
            {
                object value = GetValueForProperty(property.PropertyType, hashEntry.Value);
                property.SetValue(entity, value);
            }
        }

        return entity;
    }
    
    private object GetValueForProperty(Type propertyType, RedisValue redisValue)
    {
        if (propertyType == typeof(Guid))
        {
            return Guid.Parse((string)redisValue);
        }
        else if (propertyType == typeof(double))
        {
            return double.Parse((string)redisValue, CultureInfo.InvariantCulture);
        }
        else if (propertyType.IsEnum)
        {
            Enum.TryParse(propertyType, (string)redisValue, true, out var result);
            return result;
        }
        else
        {
            return Convert.ChangeType(redisValue, propertyType);
        }
    }
}