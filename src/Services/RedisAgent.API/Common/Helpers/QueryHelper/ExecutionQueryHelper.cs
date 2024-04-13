using StackExchange.Redis;
using Order = RedisAgent.API.Entities.Order;

namespace RedisAgent.API.Common.Helpers.QueryHelper;

public class ExecutionQueryHelper : IExecutionQueryHelper
{
    private readonly ConnectionMultiplexer _connectionMultiplexer;
    private readonly IServer _server;
    private readonly IUnitOfWork _unitOfWork;

    public ExecutionQueryHelper(IDatabaseConfiguration databaseConfiguration, IUnitOfWork unitOfWork)
    {
        _connectionMultiplexer = ConnectionMultiplexer.Connect(databaseConfiguration.ConnectionString + ",allowAdmin=true");
        _server = _connectionMultiplexer.GetServer(_connectionMultiplexer.GetEndPoints()[0]);
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }
    
    public async Task<List<dynamic>> GetProductsFromBasketByBasketId(string basketId = "6c780439-2b12-44d4-866f-bc1b500fab55")
    {
        List<HashEntry[]> productBaskets = new();
        long memoryUsage = default;
        List<dynamic> result = new();
        
        List<RedisKey> keys = _server.Keys(pattern: $"ProductBasket:*").ToList();

        foreach (var key in keys)
        {
            HashEntry[] hashEntries = await _connectionMultiplexer.GetDatabase().HashGetAllAsync(key);
            memoryUsage += GetKeyMemoryUsage(key);

            if (hashEntries.Any(hashEntry => hashEntry.Value == basketId))
            {
                productBaskets.Add(hashEntries);
            }
        }

        foreach (var productId in productBaskets.Select(productBasket => productBasket.FirstOrDefault(property => property.Name == "ProductId")))
        {
            Product? product = await _unitOfWork.Products.GetByKeyAsync($"{nameof(Product)}:{productId.Value}");
            memoryUsage += GetKeyMemoryUsage($"{nameof(Product)}:{productId.Value}");

            if (product is not null)
            {
                var resultObject = new
                {
                    Product = product,
                    BasketId = basketId
                };
                result.Add(resultObject);
            }
        }

        double keyMemoryUsage = (double)memoryUsage / 1024;
        var resourcesResult = new
        {
            KeysMemoryUsage = keyMemoryUsage
        };
        
        result.Add(resourcesResult);

        return result;
    }
    
    public async Task<List<dynamic>> GetProductsFromBasketWithUserDetails()
    {
        List<RedisKey> keys = _server.Keys(pattern: $"ProductBasket:*").ToList();
        List<dynamic> result = new();
        long memoryUsage = default;

        List<RedisValue> uniqueProductIds = new();
        List<RedisValue> uniqueBasketIds = new();
        List<Guid> uniqueUserIds = new();
    
        foreach (var key in keys)
        {
            HashEntry[] hashEntries = await _connectionMultiplexer.GetDatabase().HashGetAllAsync(key);
            memoryUsage += GetKeyMemoryUsage(key);
            
            foreach (var hashEntry in hashEntries)
            {
                if (hashEntry.Name == "ProductId" && !uniqueProductIds.Contains(hashEntry.Value))
                {
                    uniqueProductIds.Add(hashEntry.Value);
                }

                if (hashEntry.Name == "BasketId" && !uniqueBasketIds.Contains(hashEntry.Value))
                {
                    uniqueBasketIds.Add(hashEntry.Value);
                }
            }
        }
        
        foreach (var basketId in uniqueBasketIds)
        {
            Basket? basket = await _unitOfWork.Baskets.GetByKeyAsync($"{nameof(Basket)}:{basketId}");
            memoryUsage += GetKeyMemoryUsage($"{nameof(Basket)}:{basketId}");

            if (basket is not null)
            {
                uniqueUserIds.Add(basket.UserId);
            }
        }
        
        foreach (var productId in uniqueProductIds)
        {
            Random rand = new Random();
            Product? product = await _unitOfWork.Products.GetByKeyAsync($"{nameof(Product)}:{productId}");
            memoryUsage += GetKeyMemoryUsage($"{nameof(Product)}:{productId}");

            if (product is not null)
            {
                int randomIndex = rand.Next(0, uniqueUserIds.Count);
                Guid randomUserId = uniqueUserIds[randomIndex];
                
                var resultObject = new
                {
                    Product = product,
                    UserId = randomUserId
                };
                
                result.Add(resultObject);
            }
        }
        
        double keyMemoryUsage = (double)memoryUsage / 1024;
        var resourcesResult = new
        {
            KeysMemoryUsage = keyMemoryUsage
        };
        
        result.Add(resourcesResult);
    
        return result;
    }

    public async Task<dynamic> CountTotalPriceQuantityByBasketId(string basketId = "6c780439-2b12-44d4-866f-bc1b500fab55")
    {
        double totalSum = default;
        long memoryUsage = default;
        
        List<RedisKey> keys = _server.Keys(pattern: $"ProductBasket:*").ToList();

        foreach (var key in keys)
        {
            HashEntry[] hashEntries = await _connectionMultiplexer.GetDatabase().HashGetAllAsync(key);
            memoryUsage += GetKeyMemoryUsage(key);

            if (hashEntries.Any(hashEntry => hashEntry.Value == basketId))
            {
                string priceStr = hashEntries.FirstOrDefault(hashEntry => hashEntry.Name == "Price").Value.ToString();
                double quantity = Convert.ToDouble(hashEntries.FirstOrDefault(hashEntry => hashEntry.Name == "Quantity").Value);

                if (double.TryParse(priceStr.Replace(',', '.'), NumberStyles.Float, CultureInfo.InvariantCulture, out double price))
                {
                    totalSum += price * quantity;
                }
            }
        }
        
        double keyMemoryUsage = (double)memoryUsage / 1024;
        
        var result = new
        {
            TotalSum = totalSum,
            KeysMemoryUsage = keyMemoryUsage
        };
        
        return result;
    }

    public async Task<List<dynamic>> GetProductsByCategory(string categoryName = "Smartphones")
    {
        List<RedisKey> keys = _server.Keys(pattern: $"Product:*").ToList();
        List<dynamic> result = new();
        long memoryUsage = default;
        List<Product> products = new();
        
        foreach (var key in keys)
        {
            HashEntry[] hashEntries = await _connectionMultiplexer.GetDatabase().HashGetAllAsync(key);
            memoryUsage += GetKeyMemoryUsage(key);
            
            foreach (var hashEntry in hashEntries)
            {
                if (hashEntry.Name == "Id")
                {
                    Product? product = await _unitOfWork.Products.GetByKeyAsync($"{nameof(Product)}:{hashEntry.Value}");
                    memoryUsage += GetKeyMemoryUsage($"{nameof(Product)}:{hashEntry.Value}");
                    
                    if (product is not null)
                    {
                        products.Add(product);
                    }
                }
            }
        }
        
        foreach (var categoryId in products.Select(product => product.CategoryId))
        {
            Category? category = await _unitOfWork.Categories.GetByKeyAsync($"{nameof(Category)}:{categoryId}");
            memoryUsage += GetKeyMemoryUsage($"{nameof(Category)}:{categoryId}");

            if (category is not null && category.Name == categoryName)
            {
                products = products.Where(product => product.CategoryId == category.Id).OrderBy(product => product.Price).ToList();

                foreach (var product in products)
                {
                    var resultObject = new
                    {
                        Product = product,
                        Category = category
                    };
                    result.Add(resultObject);
                }
            }
        }
        
        double keyMemoryUsage = (double)memoryUsage / 1024;
        
        var resourcesResult = new
        {
            KeysMemoryUsage = keyMemoryUsage
        };
        
        result.Add(resourcesResult);

        return result;
    }

    public async Task<List<dynamic>> GetProductsByPrice(double startPrice = 1000.99, double endPrice = 1799.00)
    {
        List<RedisKey> keys = _server.Keys(pattern: $"Product:*").ToList();
        List<dynamic> result = new();
        long memoryUsage = default;
        
        foreach (var key in keys)
        {
            HashEntry[] hashEntries = await _connectionMultiplexer.GetDatabase().HashGetAllAsync(key);
            memoryUsage += GetKeyMemoryUsage(key);
            
            RedisValue productId = default;
            foreach (var hashEntry in hashEntries)
            {
                if (hashEntry.Name == "Id")
                {
                    productId = hashEntry.Value;
                }
                
                if (hashEntry.Name == "Price")
                {
                    if (double.TryParse(hashEntry.Value.ToString().Replace(',', '.'), NumberStyles.Float, CultureInfo.InvariantCulture, out double convertedPrice))
                    {
                        if (convertedPrice > startPrice && convertedPrice < endPrice)
                        {
                            Product? product = await _unitOfWork.Products.GetByKeyAsync($"{nameof(Product)}:{productId}");
                            memoryUsage += GetKeyMemoryUsage($"{nameof(Product)}:{productId}");
                        
                            if (product is not null)
                            {
                                var resultObject = new
                                {
                                    Product = product
                                };
                                result.Add(resultObject);
                            }
                        }
                    }
                }
            }
        }
        
        double keyMemoryUsage = (double)memoryUsage / 1024;
        
        var resourcesResult = new
        {
            KeysMemoryUsage = keyMemoryUsage
        };
        
        result.Add(resourcesResult);

        return result;
    }

    public async Task<List<dynamic>> GetProductsWithLikeFilter(string productName = "Apple", double startPrice = 1000.00, double endPrice = 2000.00)
    {
        List<RedisKey> keys = _server.Keys(pattern: $"Product:*").ToList();
        List<Product> appleProducts = new();
        List<dynamic> result = new();
        long memoryUsage = default;

        foreach (var key in keys)
        {
            HashEntry[] hashEntries = await _connectionMultiplexer.GetDatabase().HashGetAllAsync(key);
            memoryUsage += GetKeyMemoryUsage(key);

            RedisValue productId = default;
            foreach (var hashEntry in hashEntries)
            {
                if (hashEntry.Name == "Id")
                {
                    productId = hashEntry.Value;
                }

                if (hashEntry.Name == "Name" && hashEntry.Value.ToString().Contains(productName))
                {
                    Product? product = await _unitOfWork.Products.GetByKeyAsync($"{nameof(Product)}:{productId}");
                    memoryUsage += GetKeyMemoryUsage($"{nameof(Product)}:{productId}");

                    if (product is not null)
                    {
                        appleProducts.Add(product);
                    }
                }
            }
        }

        foreach (var product in appleProducts)
        {
            if (product.Price > startPrice && product.Price < endPrice)
            {
                var resultObject = new
                {
                    Product = product
                };
                result.Add(resultObject);
            }
        }
        
        double keyMemoryUsage = (double)memoryUsage / 1024;
        
        var resourcesResult = new
        {
            KeysMemoryUsage = keyMemoryUsage
        };
        
        result.Add(resourcesResult);

        return result;
    }

    public async Task<List<dynamic>> CountTotalSpentForEachUser(string orderStatus = "Paid")
    {
        List<RedisKey> keys = _server.Keys(pattern: $"Order:*").ToList();
        List<dynamic> result = new();
        long memoryUsage = default;
        List<RedisValue> uniqueUserIds = new();
        List<Order> orders = new();
        double totalSum = default;
        
        foreach (var key in keys)
        {
            HashEntry[] hashEntries = await _connectionMultiplexer.GetDatabase().HashGetAllAsync(key);
            memoryUsage += GetKeyMemoryUsage(key);
            
            foreach (var hashEntry in hashEntries)
            {
                if (hashEntry.Name == "Id")
                {
                    Order? order = await _unitOfWork.Orders.GetByKeyAsync($"{nameof(Order)}:{hashEntry.Value}");
                    memoryUsage += GetKeyMemoryUsage($"{nameof(Order)}:{hashEntry.Value}");

                    if (order is not null)
                    {
                        orders.Add(order);
                    }
                }
                
                if (hashEntry.Name == "UserId" && !uniqueUserIds.Contains(hashEntry.Value))
                {
                    uniqueUserIds.Add(hashEntry.Value);
                }
            }
        }

        foreach (var order in orders.Where(order => order.OrderStatus != OrderStatus.Paid))
        {
            orders.Remove(order);
        }

        foreach (var uniqueUserId in uniqueUserIds)
        {
            Guid userId = new(uniqueUserId.ToString());
            List<Order> ordersWithCurrentUserId = orders.Where(order => order.UserId == userId).ToList();
            totalSum += ordersWithCurrentUserId.Sum(order => order.TotalPrice);
            
            User? user = await _unitOfWork.Users.GetByKeyAsync($"{nameof(User)}:{userId}");
            memoryUsage += GetKeyMemoryUsage($"{nameof(User)}:{userId}");

            if (user is not null)
            {
                var resultObject = new
                {
                    TotalSum = totalSum,
                    User = user
                };
                result.Add(resultObject);
            }

            totalSum = 0;
        }
        
        double keyMemoryUsage = (double)memoryUsage / 1024;
        
        var resourcesResult = new
        {
            KeysMemoryUsage = keyMemoryUsage
        };
        
        result.Add(resourcesResult);

        return result;
    }

    public double GetRedisCacheSize()
    {
        var info = _server.Info();
        
        return ParseInfoOutput(info, "Memory","used_memory_dataset") / (1024 * 1024);
    }

    private long GetKeyMemoryUsage(string key)
    {
        IDatabase database = _connectionMultiplexer.GetDatabase();
        
        return (long)database.Execute("MEMORY", "USAGE", key);
    }
    
    private double ParseInfoOutput(IGrouping<string, KeyValuePair<string, string>>[] infoGroups, string mainKey, string key)
    {
        foreach (var group in infoGroups)
        {
            if (group.Key == mainKey)
            {
                var value = group.FirstOrDefault(pair => pair.Key == key);
                
                if (double.TryParse(value.Value, out double result))
                {
                    return Math.Round(result, 3);
                }
                else
                {
                    throw new InvalidOperationException($"Failed to parse value for key '{key}'.");
                }
            }
        }

        throw new InvalidOperationException($"Key '{key}' not found in INFO output.");
    }
    
    
}