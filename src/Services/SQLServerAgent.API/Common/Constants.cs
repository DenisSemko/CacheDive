namespace SQLServerAgent.API.Common;

public static class Constants
{
    public const string EntityNamespace = "SQLServerAgent.API.Models.";

    public const string ConnectionString =
        "Server=localhost;Database=CacheDiveSQLServerDb;User Id=sa;Password=mcbv3n12!;TrustServerCertificate=True;";

    public static class Queries
    {
        public const string ClearBufferCacheQuery = "DBCC DROPCLEANBUFFERS;";
        public const string ClearProcedureCacheQuery = "DBCC FREEPROCCACHE;";
        public const string GetExecutionPlanQuery = @"
                SELECT deqp.query_plan, dest.text
                FROM sys.dm_exec_query_stats AS deqs 
                CROSS APPLY sys.dm_exec_sql_text(deqs.sql_handle) AS dest
                CROSS APPLY sys.dm_exec_query_plan(deqs.plan_handle) AS deqp 
                WHERE dest.text LIKE '{0}%';";
        public const string GetCacheHitMissQuery = @"
                DECLARE @CacheHits INT, @CacheMisses INT;

                SELECT 
                    @CacheHits = deqs.total_worker_time,
                    @CacheMisses = deqs.execution_count - 1
                FROM sys.dm_exec_query_stats AS deqs
                CROSS APPLY sys.dm_exec_sql_text(deqs.sql_handle) AS dest
                WHERE dest.text LIKE N'%{0}%';

                DECLARE @CacheHitRatio DECIMAL(5, 2), @CacheMissRatio DECIMAL(5, 2);

                SET @CacheHitRatio = CASE WHEN @CacheHits + @CacheMisses = 0 THEN 0 ELSE (@CacheHits * 1.0 / (@CacheHits + @CacheMisses)) * 100 END;
                SET @CacheMissRatio = CASE WHEN @CacheHits + @CacheMisses = 0 THEN 0 ELSE (@CacheMisses * 1.0 / (@CacheHits + @CacheMisses)) * 100 END;

                SELECT
                    @CacheHitRatio AS 'Cache Hit Ratio',
                    @CacheMissRatio AS 'Cache Miss Ratio';
                ";
        public const string GetCacheSizeQuery = @"
                SELECT (sum(size_in_bytes) / 1024) AS 'Procedure Cache Size (MB)'
                FROM sys.dm_exec_cached_plans;";

        public const string SelectProductsByBasketId = @"SELECT p.Id AS Product_Id, p.Name as Product_Name, 
                   pb.Quantity, 
                   b.Id AS Basket_Id 
            FROM Products p 
            JOIN ProductBaskets pb ON p.Id = pb.ProductId 
            JOIN Baskets b ON pb.BasketId = b.Id 
            WHERE b.Id = '6c780439-2b12-44d4-866f-bc1b500fab55';";

        public const string SelectProductsByUserId = @"
            SELECT p.Id, p.Name, pb.Quantity, u.Id 
            FROM Products p 
            JOIN ProductBaskets pb on p.Id = pb.ProductId 
            JOIN Baskets b on pb.BasketId = b.Id 
            JOIN Users u on b.UserId = u.Id;";

        public const string CountTotalPriceTotalQuantityByBasketId = @"
            SELECT SUM(pb.Price * pb.Quantity)  
            FROM ProductBaskets pb 
            JOIN Baskets b ON pb.BasketId = b.Id 
            WHERE b.Id = '6c780439-2b12-44d4-866f-bc1b500fab55';";

        public const string SelectProductsByCategory = @"
            SELECT p.Id, p.Name 
            FROM Products p  
            JOIN Categories c ON p.CategoryId = c.Id 
            WHERE c.Name = 'Smartphones' ORDER BY p.Price;";

        public const string SelectProductsWithPriceFiltered = @"
            SELECT p.Id, p.NAME  
            FROM Products p 
            WHERE p.Price > 1000.99 AND Price < 1799.00;";

        public const string SelectProductsWithLikeFiltering = @"
            SELECT p.Id, p.Name 
            FROM Products p 
            WHERE p.NAME LIKE '%Apple%' AND p.Price BETWEEN 1000.00 AND 2000.00 
            ORDER BY p.Name;";

        public const string SumTotalOrderPrice = @"
            SELECT SUM(o.TotalPrice), u.Name, u.Email 
            FROM Orders o 
            JOIN Users u ON u.Id = o.UserId 
            WHERE o.OrderStatus = 4
            GROUP BY u.Name, u.Email;";
    }

    public static class QueryPlan
    {
        public const string Title = "query_plan";
        public const string RetrievedFromCache = "RetrievedFromCache=\"true\"";
        public const string QueryPlanTag = "<QueryPlan";
        public const string CacheHitColumnName = "Cache Hit Ratio";
        public const string CacheMissColumnName = "Cache Miss Ratio";
        public const string CachedPlanSize = "CachedPlanSize";
        public const string CompileCPU = "CompileCPU";
        public const string CompileMemory = "CompileMemory";
        public const string ProcedureCacheColumnName = "Procedure Cache Size (MB)";
    }
}