using EventBus.Messages.Events.Response;

namespace MongoAgent.API.Consumer;

public class GetMongoDbExecutionConsumer : IConsumer<GetMongoDbExecutionRequest>
{
    private readonly IExecutionQueryHelper _executionQueryHelper;
    private readonly IExecutionTimeHelper _executionTimeHelper;

    public GetMongoDbExecutionConsumer(IExecutionQueryHelper executionQueryHelper, IExecutionTimeHelper executionTimeHelper)
    {
        _executionQueryHelper = executionQueryHelper ?? throw new ArgumentNullException(nameof(executionQueryHelper));
        _executionTimeHelper = executionTimeHelper ?? throw new ArgumentNullException(nameof(executionTimeHelper));
    }


    public async Task Consume(ConsumeContext<GetMongoDbExecutionRequest> context)
    {
        TimeSpan resultTime = TimeSpan.Zero;
        Result commandStats = default!;
        CacheStats cacheStats = default!;
        List<BsonDocument> executionResultData = new List<BsonDocument>();

        //profiler enabled
        BsonDocument profilerCommand = new ("profile", 2);
        BsonDocument profilingStatus = await _executionQueryHelper.ExecuteCommandWithResult(profilerCommand);
        
        if (profilingStatus[3] == 1)
        {
            switch (context.Message.ExperimentType)
            {
                case ExperimentType.BasketBasketId:
                    await CleanCache(context.Message.IsCacheCleaned, nameof(ProductBasket));
                
                    resultTime = await _executionTimeHelper.MeasureExecutionTime(async () =>
                    {
                        executionResultData = await _executionQueryHelper.GetProductsFromBasketByBasketId();
                    }, context.Message.QueryExecutionNumber);

                    if (executionResultData.Any())
                    {
                        BsonElement pipelineSize = executionResultData
                            .SelectMany(doc => doc.Elements)
                            .FirstOrDefault(element => element.Name == "pipelineSizeInKb");
                        BsonElement pipeline = executionResultData
                            .SelectMany(doc => doc.Elements)
                            .FirstOrDefault(element => element.Name == "pipeline");
                        commandStats = await _executionQueryHelper.GetCommandStats((double)pipelineSize.Value, pipeline.Value.ToString(), context.Message.ExperimentType);
                        cacheStats = await _executionQueryHelper.CountCacheStats();
                    }
                    break;
                case ExperimentType.BasketUserId:
                    await CleanCache(context.Message.IsCacheCleaned, nameof(ProductBasket));
                
                    resultTime = await _executionTimeHelper.MeasureExecutionTime(async () =>
                    {
                        executionResultData = await _executionQueryHelper.GetProductsFromBasketWithUserDetails();
                    }, context.Message.QueryExecutionNumber);

                    if (executionResultData.Any())
                    {
                        BsonElement pipelineSize = executionResultData
                            .SelectMany(doc => doc.Elements)
                            .FirstOrDefault(element => element.Name == "pipelineSizeInKb");
                        BsonElement pipeline = executionResultData
                            .SelectMany(doc => doc.Elements)
                            .FirstOrDefault(element => element.Name == "pipeline");
                        commandStats = await _executionQueryHelper.GetCommandStats((double)pipelineSize.Value, pipeline.Value.ToString(), context.Message.ExperimentType);
                        cacheStats = await _executionQueryHelper.CountCacheStats();
                    }
                    break;
                case ExperimentType.BasketTotalPrice:
                    await CleanCache(context.Message.IsCacheCleaned, nameof(Basket));
                    
                    resultTime = await _executionTimeHelper.MeasureExecutionTime(async () =>
                    {
                        executionResultData = await _executionQueryHelper.CountTotalPriceQuantityByBasketId();
                    }, context.Message.QueryExecutionNumber);

                    if (executionResultData.Any())
                    {
                        BsonElement pipelineSize = executionResultData
                            .SelectMany(doc => doc.Elements)
                            .FirstOrDefault(element => element.Name == "pipelineSizeInKb");
                        BsonElement pipeline = executionResultData
                            .SelectMany(doc => doc.Elements)
                            .FirstOrDefault(element => element.Name == "pipeline");
                        commandStats = await _executionQueryHelper.GetCommandStats((double)pipelineSize.Value, pipeline.Value.ToString(), context.Message.ExperimentType);
                        cacheStats = await _executionQueryHelper.CountCacheStats();
                    }
                    break;
                case ExperimentType.ProductSmartphones:
                    await CleanCache(context.Message.IsCacheCleaned, nameof(Product));
                    
                    resultTime = await _executionTimeHelper.MeasureExecutionTime(async () =>
                    {
                        executionResultData = await _executionQueryHelper.GetProductsByCategory();
                    }, context.Message.QueryExecutionNumber);

                    if (executionResultData.Any())
                    {
                        BsonElement pipelineSize = executionResultData
                            .SelectMany(doc => doc.Elements)
                            .FirstOrDefault(element => element.Name == "pipelineSizeInKb");
                        BsonElement pipeline = executionResultData
                            .SelectMany(doc => doc.Elements)
                            .FirstOrDefault(element => element.Name == "pipeline");
                        commandStats = await _executionQueryHelper.GetCommandStats((double)pipelineSize.Value, pipeline.Value.ToString(), context.Message.ExperimentType);
                        cacheStats = await _executionQueryHelper.CountCacheStats();
                    }
                    break;
                case ExperimentType.ProductPrice:
                    await CleanCache(context.Message.IsCacheCleaned, nameof(Product));
                    
                    resultTime = await _executionTimeHelper.MeasureExecutionTime(async () =>
                    {
                        executionResultData = await _executionQueryHelper.GetProductsByPrice();
                    }, context.Message.QueryExecutionNumber);

                    if (executionResultData.Any())
                    {
                        BsonElement pipelineSize = executionResultData
                            .SelectMany(doc => doc.Elements)
                            .FirstOrDefault(element => element.Name == "pipelineSizeInKb");
                        BsonElement pipeline = executionResultData
                            .SelectMany(doc => doc.Elements)
                            .FirstOrDefault(element => element.Name == "pipeline");
                        commandStats = await _executionQueryHelper.GetCommandStats((double)pipelineSize.Value, pipeline.Value.ToString(), context.Message.ExperimentType);
                        cacheStats = await _executionQueryHelper.CountCacheStats();
                    }
                    break;
                case ExperimentType.ProductLike:
                    await CleanCache(context.Message.IsCacheCleaned, nameof(Product));
                    
                    resultTime = await _executionTimeHelper.MeasureExecutionTime(async () =>
                    {
                        executionResultData = await _executionQueryHelper.GetProductsWithLikeFilter();
                    }, context.Message.QueryExecutionNumber);

                    if (executionResultData.Any())
                    {
                        BsonElement pipelineSize = executionResultData
                            .SelectMany(doc => doc.Elements)
                            .FirstOrDefault(element => element.Name == "pipelineSizeInKb");
                        BsonElement pipeline = executionResultData
                            .SelectMany(doc => doc.Elements)
                            .FirstOrDefault(element => element.Name == "pipeline");
                        commandStats = await _executionQueryHelper.GetCommandStats((double)pipelineSize.Value, pipeline.Value.ToString(), context.Message.ExperimentType);
                        cacheStats = await _executionQueryHelper.CountCacheStats();
                    }
                    break;
                case ExperimentType.OrderGroupBy:
                    await CleanCache(context.Message.IsCacheCleaned, nameof(Order));
                    
                    resultTime = await _executionTimeHelper.MeasureExecutionTime(async () =>
                    {
                        executionResultData = await _executionQueryHelper.CountTotalSpentForEachUser();
                    }, context.Message.QueryExecutionNumber);

                    if (executionResultData.Any())
                    {
                        BsonElement pipelineSize = executionResultData
                            .SelectMany(doc => doc.Elements)
                            .FirstOrDefault(element => element.Name == "pipelineSizeInKb");
                        BsonElement pipeline = executionResultData
                            .SelectMany(doc => doc.Elements)
                            .FirstOrDefault(element => element.Name == "pipeline");
                        commandStats = await _executionQueryHelper.GetCommandStats((double)pipelineSize.Value, pipeline.Value.ToString(), context.Message.ExperimentType);
                        cacheStats = await _executionQueryHelper.CountCacheStats();
                    }
                    break;
            }
        }

        await context.RespondAsync(new GetMongoDbExecutionResponse()
        {
            Query = commandStats.Pipeline.ToString(),
            QueryExecutionNumber = context.Message.QueryExecutionNumber,
            IsExecutedFromCache = commandStats.IsExecutedFromCache,
            CacheHitRate = cacheStats.CacheHitRate,
            CacheMissRate = cacheStats.CacheMissRate,
            QueryExecutionTime = resultTime.ToString(),
            Resources = commandStats.Resources,
            CacheSize = cacheStats.CacheSize
        });
    }

    private async Task CleanCache(bool isCacheCleaned, string entityName)
    {
        if (isCacheCleaned)
        {
            BsonDocument basketCacheCleanCommand = new ("planCacheClear", entityName);
            await _executionQueryHelper.ExecuteCommandWithResult(basketCacheCleanCommand);
        }
    }
}