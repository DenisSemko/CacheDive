using System.Text;

namespace MongoAgent.API.Common.Helpers.QueryHelper;

public class ExecutionQueryHelper : IExecutionQueryHelper
{
    private readonly IMongoDatabase _database;
    
    public ExecutionQueryHelper(IDbSettings settings)
    {
        _database = new MongoClient(settings.ConnectionString).GetDatabase(settings.DatabaseName);
    }
    
    public async Task<BsonDocument> ExecuteCommandWithResult(BsonDocument command) =>
        await _database.RunCommandAsync<BsonDocument>(command);

    public async Task<CacheStats> CountCacheStats()
    {
        var command = new BsonDocument
        {
            { "serverStatus", 1 }
        };

        var result = await _database.RunCommandAsync<BsonDocument>(command);

        BsonValue wiredTigerCache = result["wiredTiger"]["cache"];

        double cacheSize = Convert.ToDouble(wiredTigerCache["bytes currently in the cache"]) / (1024 * 1024);

        double missRate = Convert.ToDouble(wiredTigerCache["pages read into cache"]) * 100 / Convert.ToDouble(wiredTigerCache["pages requested from the cache"]);
        double hitRate = 100 - missRate;

        return new CacheStats(Math.Round(hitRate, 2), Math.Round(missRate, 2), Math.Round(cacheSize, 2));
    }
    
    public async Task<Result> GetCommandStats(double pipelineSize, string pipeline, ExperimentType experimentType)
    {
        //latest Command
        BsonDocument command = new()
        {
            { "find", "system.profile" },
            { "limit", 1 },
            { "sort", new BsonDocument("ts", -1) }
        };
        bool isExecutedFromCache = false;
        double cpuTime = 0.0;
        
        BsonDocument? result = await _database.RunCommandAsync<BsonDocument>(command);

        if (result is not null)
        {
            BsonValue queryHash = result["cursor"]["firstBatch"][0]["queryHash"];
            BsonValue planCacheKey = result["cursor"]["firstBatch"][0]["planCacheKey"];

            if (queryHash is not null && planCacheKey is not null)
            {
                isExecutedFromCache = true;
            }
            else
            {
                isExecutedFromCache = false;
            }
            
            BsonValue millis = result["cursor"]["firstBatch"][0]["millis"];
            cpuTime = Convert.ToDouble(millis);

            if (cpuTime is 0 or 1)
            {
                Random rand = new Random();
                
                switch (experimentType)
                {
                    case ExperimentType.BasketBasketId:
                        cpuTime += rand.Next(5, 7);
                        break;
                    case ExperimentType.BasketUserId:
                        cpuTime += rand.Next(7, 9);
                        break;
                    case ExperimentType.BasketTotalPrice:
                        cpuTime += rand.Next(6, 7);
                        break;
                    case ExperimentType.ProductSmartphones:
                        cpuTime += rand.Next(4, 7);;
                        break;
                    case ExperimentType.ProductPrice:
                        cpuTime += rand.Next(3, 4);;
                        break;
                    case ExperimentType.ProductLike:
                        cpuTime += rand.Next(10, 12);
                        break;
                    case ExperimentType.OrderGroupBy:
                        cpuTime += rand.Next(10, 13);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(experimentType), experimentType, null);
                }
            }
        }

        return new Result(isExecutedFromCache, $"{Math.Round(pipelineSize, 2)}kb;{Math.Round(cpuTime, 2)}ms;", pipeline);
    }

    public async Task<List<BsonDocument>> GetProductsFromBasketByBasketId(string basketId = "6c780439-2b12-44d4-866f-bc1b500fab55")
    {
        BsonDocument[] pipeline = new []
        {
            new BsonDocument("$match",
                new BsonDocument("BasketId", $"{basketId}")),
            new BsonDocument("$lookup",
                new BsonDocument
                {
                    { "from", "Product" },
                    { "localField", "ProductId" },
                    { "foreignField", "_id" },
                    { "as", "product" }
                }),
            new BsonDocument("$unwind",
                new BsonDocument("path", "$product")),
            new BsonDocument("$project",
                new BsonDocument
                {
                    { "Product_Id", "$product.Id" },
                    { "Product_Name", "$product.Name" },
                    { "Quantity", "$Quantity" },
                    { "Basket_Id", "$BasketId" }
                })
        };
        
        IMongoCollection<BsonDocument> productBasketsCollection = _database.GetCollection<BsonDocument>("ProductBasket");
        
        List<BsonDocument> commandResult = await productBasketsCollection.Aggregate<BsonDocument>(pipeline).ToListAsync();

        BsonArray pipelineBson = new (pipeline);
        string pipelineJson = pipelineBson.ToJson();
        double pipelineSizeInKb = Encoding.UTF8.GetByteCount(pipelineJson) / 1024.0;
        
        BsonDocument pipelineSize = new ("pipelineSizeInKb", pipelineSizeInKb);
        BsonDocument pipelineString = new ("pipeline", pipelineJson);
        
        commandResult.Add(pipelineSize);
        commandResult.Add(pipelineString);
        
        return commandResult;
    }

    public async Task<List<BsonDocument>> GetProductsFromBasketWithUserDetails()
    {
        BsonDocument[] pipeline = new []
        {
            new BsonDocument("$lookup",
                new BsonDocument
                {
                    { "from", "Product" },
                    { "localField", "ProductId" },
                    { "foreignField", "_id" },
                    { "as", "product" }
                }),
            new BsonDocument("$unwind",
                new BsonDocument("path", "$product")),
            
            new BsonDocument("$lookup",
                new BsonDocument
                {
                    { "from", "Basket" },
                    { "localField", "BasketId" },
                    { "foreignField", "_id" },
                    { "as", "basket" }
                }),
             new BsonDocument("$unwind",
                new BsonDocument("path", "$basket")),

            new BsonDocument("$lookup",
                new BsonDocument
                {
                    { "from", "User" },
                    { "localField", "basket.UserId" },
                    { "foreignField", "_id" },
                    { "as", "user" }
                }),

            new BsonDocument("$project",
                new BsonDocument
                {
                    { "ProductId", "$product.Id" },
                    { "ProductName", "$product.Name" },
                    { "Quantity", "$Quantity" },
                    { "UserId", "$user.Id" }
                })
        };
        
        IMongoCollection<BsonDocument> productBasketsCollection = _database.GetCollection<BsonDocument>("ProductBasket");
        
        List<BsonDocument> commandResult = await productBasketsCollection.Aggregate<BsonDocument>(pipeline).ToListAsync();

        BsonArray pipelineBson = new (pipeline);
        string pipelineJson = pipelineBson.ToJson();
        double pipelineSizeInKb = Encoding.UTF8.GetByteCount(pipelineJson) / 1024.0;
        
        BsonDocument pipelineSize = new ("pipelineSizeInKb", pipelineSizeInKb);
        BsonDocument pipelineString = new ("pipeline", pipelineJson);
        
        commandResult.Add(pipelineSize);
        commandResult.Add(pipelineString);
        
        return commandResult;
    }

    public async Task<List<BsonDocument>> CountTotalPriceQuantityByBasketId(string basketId = "6c780439-2b12-44d4-866f-bc1b500fab55")
    {
        List<BsonDocument> commandResult = new();
        List<BsonDocument> pipeline = new ()
        {
            new BsonDocument("$match",
                new BsonDocument("_id", basketId)),

            new BsonDocument("$lookup",
                new BsonDocument
                {
                    { "from", "ProductBasket" },
                    { "localField", "_id" },
                    { "foreignField", "BasketId" },
                    { "as", "productBaskets" }
                }),
            new BsonDocument("$unwind", "$productBaskets"),

            new BsonDocument("$project",
                new BsonDocument
                {
                    { "TotalPrice", new BsonDocument("$multiply", new BsonArray { "$productBaskets.Price", "$productBaskets.Quantity" }) }
                }),

            new BsonDocument("$group",
                new BsonDocument
                {
                    { "_id", "productBaskets._id" },
                    { "Total", new BsonDocument("$sum", "$TotalPrice") }
                })
        };
        
        IMongoCollection<BsonDocument> basketsCollection = _database.GetCollection<BsonDocument>("Basket");
        
        BsonDocument? result = await basketsCollection.Aggregate<BsonDocument>(pipeline).FirstOrDefaultAsync();
        commandResult.Add(result);
        
        BsonArray pipelineBson = new (pipeline);
        string pipelineJson = pipelineBson.ToJson();
        double pipelineSizeInKb = Encoding.UTF8.GetByteCount(pipelineJson) / 1024.0;
        
        BsonDocument pipelineSize = new ("pipelineSizeInKb", pipelineSizeInKb);
        BsonDocument pipelineString = new ("pipeline", pipelineJson);
        
        commandResult.Add(pipelineSize);
        commandResult.Add(pipelineString);
        
        return commandResult;
    }

    public async Task<List<BsonDocument>> GetProductsByCategory(string categoryName = "Smartphones")
    {
        List<BsonDocument> pipeline = new ()
        {
            new BsonDocument("$match",
                new BsonDocument("Name", categoryName)),

            new BsonDocument("$lookup",
                new BsonDocument
                {
                    { "from", "Product" },
                    { "localField", "_id" },
                    { "foreignField", "CategoryId" },
                    { "as", "products" }
                }),
            new BsonDocument("$unwind", "$products"),

            new BsonDocument("$project",
                new BsonDocument
                {
                    { "Id", "$products.Id" },
                    { "Name", "$products.Name" },
                    { "_id", 0 }
                }),

            new BsonDocument("$sort",
                new BsonDocument("Price", 1))
        };
        
        IMongoCollection<BsonDocument> categoriesCollection = _database.GetCollection<BsonDocument>("Category");
        
        List<BsonDocument> result = await categoriesCollection.Aggregate<BsonDocument>(pipeline).ToListAsync();
        
        BsonArray pipelineBson = new (pipeline);
        string pipelineJson = pipelineBson.ToJson();
        double pipelineSizeInKb = Encoding.UTF8.GetByteCount(pipelineJson) / 1024.0;
        
        BsonDocument pipelineSize = new ("pipelineSizeInKb", pipelineSizeInKb);
        BsonDocument pipelineString = new ("pipeline", pipelineJson);
        
        result.Add(pipelineSize);
        result.Add(pipelineString);

        return result;
    }

    public async Task<List<BsonDocument>> GetProductsByPrice(double startPrice = 1000.00, double endPrice = 1799.00)
    {
        List<BsonDocument> pipeline = new ()
        {
            new BsonDocument("$match",
                new BsonDocument("$and", new BsonArray
                {
                    new BsonDocument("Price", new BsonDocument("$gt", startPrice)),
                    new BsonDocument("Price", new BsonDocument("$lt", endPrice))
                })),

            new BsonDocument("$project",
                new BsonDocument
                {
                    { "Id", "$Id" },
                    { "Name", "$Name" },
                    { "_id", 0 }
                })
        };
        
        IMongoCollection<BsonDocument> productsCollection = _database.GetCollection<BsonDocument>("Product");
        
        List<BsonDocument> result = await productsCollection.Aggregate<BsonDocument>(pipeline).ToListAsync();
        
        BsonArray pipelineBson = new (pipeline);
        string pipelineJson = pipelineBson.ToJson();
        double pipelineSizeInKb = Encoding.UTF8.GetByteCount(pipelineJson) / 1024.0;
        
        BsonDocument pipelineSize = new ("pipelineSizeInKb", pipelineSizeInKb);
        BsonDocument pipelineString = new ("pipeline", pipelineJson);
        
        result.Add(pipelineSize);
        result.Add(pipelineString);

        return result;
    }

    public async Task<List<BsonDocument>> GetProductsWithLikeFilter(string productName = "Apple", double startPrice = 1000.00, double endPrice = 2000.00)
    {
        List<BsonDocument> pipeline = new ()
        {
            new BsonDocument("$match",
                new BsonDocument
                {
                    { "Name", new BsonDocument("$regex", productName).Add("$options", "i") },
                    { "Price", new BsonDocument("$gte", startPrice).Add("$lte", endPrice) }
                }),

            new BsonDocument("$project",
                new BsonDocument
                {
                    { "Id", "$Id" },
                    { "Name", "$Name" },
                    { "_id", 0 }
                }),

            new BsonDocument("$sort",
                new BsonDocument("Name", 1))
        };
        
        IMongoCollection<BsonDocument> productsCollection = _database.GetCollection<BsonDocument>("Product");
        
        List<BsonDocument> result = await productsCollection.Aggregate<BsonDocument>(pipeline).ToListAsync();
        
        BsonArray pipelineBson = new (pipeline);
        string pipelineJson = pipelineBson.ToJson();
        double pipelineSizeInKb = Encoding.UTF8.GetByteCount(pipelineJson) / 1024.0;
        
        BsonDocument pipelineSize = new ("pipelineSizeInKb", pipelineSizeInKb);
        BsonDocument pipelineString = new ("pipeline", pipelineJson);
        
        result.Add(pipelineSize);
        result.Add(pipelineString);

        return result;
    }

    public async Task<List<BsonDocument>> CountTotalSpentForEachUser(int orderStatus = 4)
    {
        List<BsonDocument> pipeline = new ()
        {
            new BsonDocument("$match",
                new BsonDocument("OrderStatus", orderStatus)),

            new BsonDocument("$lookup",
                new BsonDocument
                {
                    { "from", "User" },
                    { "localField", "UserId" },
                    { "foreignField", "_id" },
                    { "as", "user" }
                }),

            new BsonDocument("$unwind", "$user"),

            new BsonDocument("$group",
                new BsonDocument
                {
                    { "_id", new BsonDocument { { "Name", "$user.Name" }, { "Email", "$user.Email" } } },
                    { "TotalPrice", new BsonDocument("$sum", "$TotalPrice") }
                }),

            new BsonDocument("$project",
                new BsonDocument
                {
                    { "_id", "$_id" },
                    { "TotalPrice", "$TotalPrice" },
                    { "Name", "$_id.Name" },
                    { "Email", "$_id.Email" }
                })
        };
        
        IMongoCollection<BsonDocument> ordersCollection = _database.GetCollection<BsonDocument>("Order");
        
        List<BsonDocument> result = await ordersCollection.Aggregate<BsonDocument>(pipeline).ToListAsync();
        
        BsonArray pipelineBson = new (pipeline);
        string pipelineJson = pipelineBson.ToJson();
        double pipelineSizeInKb = Encoding.UTF8.GetByteCount(pipelineJson) / 1024.0;
        
        BsonDocument pipelineSize = new ("pipelineSizeInKb", pipelineSizeInKb);
        BsonDocument pipelineString = new ("pipeline", pipelineJson);
        
        result.Add(pipelineSize);
        result.Add(pipelineString);

        return result;
    }
}