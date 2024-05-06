namespace MongoAgent.API.Common.Helpers.QueryHelper;

public interface IExecutionQueryHelper
{
    Task<BsonDocument> ExecuteCommandWithResult(BsonDocument command);
    Task<CacheStats> CountCacheStats();
    Task<Result> GetCommandStats(double pipelineSize, string pipeline, ExperimentType experimentType);
    Task<List<BsonDocument>> GetProductsFromBasketByBasketId(string basketId = "6c780439-2b12-44d4-866f-bc1b500fab55");
    Task<List<BsonDocument>> GetProductsFromBasketWithUserDetails();
    Task<List<BsonDocument>> CountTotalPriceQuantityByBasketId(string basketId = "6c780439-2b12-44d4-866f-bc1b500fab55");
    Task<List<BsonDocument>> GetProductsByCategory(string categoryName = "Smartphones");
    Task<List<BsonDocument>> GetProductsByPrice(double startPrice = 1000.00, double endPrice = 1799.00);
    Task<List<BsonDocument>> GetProductsWithLikeFilter(string productName = "Apple", double startPrice = 1000.00, double endPrice = 2000.00);
    Task<List<BsonDocument>> CountTotalSpentForEachUser(int orderStatus = 4);
}