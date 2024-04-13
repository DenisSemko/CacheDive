namespace RedisAgent.API.Common.Helpers.QueryHelper;

public interface IExecutionQueryHelper
{
    Task<List<dynamic>> GetProductsFromBasketByBasketId(string basketId = "6c780439-2b12-44d4-866f-bc1b500fab55");
    Task<List<dynamic>> GetProductsFromBasketWithUserDetails();
    Task<dynamic> CountTotalPriceQuantityByBasketId(string basketId = "6c780439-2b12-44d4-866f-bc1b500fab55");
    Task<List<dynamic>> GetProductsByCategory(string categoryName = "Smartphones");
    Task<List<dynamic>> GetProductsByPrice(double startPrice = 1000.99, double endPrice = 1799.00);
    Task<List<dynamic>> GetProductsWithLikeFilter(string productName = "Apple", double startPrice = 1000.00, double endPrice = 2000.00);
    Task<List<dynamic>> CountTotalSpentForEachUser(string orderStatus = "Paid");
    double GetRedisCacheSize();
}