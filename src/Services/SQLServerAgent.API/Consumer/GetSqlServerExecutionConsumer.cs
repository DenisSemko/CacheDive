namespace SQLServerAgent.API.Consumer;

public class GetSqlServerExecutionConsumer : IConsumer<GetSqlServerExecutionRequest>
{
    private readonly IExecutionQueryHelper _executionQueryHelper;
    private readonly IExecutionTimeHelper _executionTimeHelper;

    public GetSqlServerExecutionConsumer(IExecutionQueryHelper executionQueryHelper, IExecutionTimeHelper executionTimeHelper)
    {
        _executionQueryHelper = executionQueryHelper ?? throw new ArgumentNullException(nameof(executionQueryHelper));
        _executionTimeHelper = executionTimeHelper ?? throw new ArgumentNullException(nameof(executionTimeHelper));
    }

    public async Task Consume(ConsumeContext<GetSqlServerExecutionRequest> context)
    {
        string query = _executionQueryHelper.HandleExperimentType(context.Message.ExperimentType);
        
        //clear cache
        if (context.Message.IsCacheCleaned)
        {
            await _executionQueryHelper.ExecuteQuery(Constants.Queries.ClearBufferCacheQuery);
            await _executionQueryHelper.ExecuteQuery(Constants.Queries.ClearProcedureCacheQuery);
        }
        
        //main query execution
        TimeSpan resultTime = await _executionTimeHelper.MeasureExecutionTime(async () =>
        {
            await _executionQueryHelper.ExecuteQuery(query);
        }, context.Message.QueryExecutionNumber);
        
        // execution plan
        string updatedQuery = string.Format(Constants.Queries.GetExecutionPlanQuery, query.Replace("'", "''"));

        string queryPlan = await _executionQueryHelper.ExecuteQueryWithReader(updatedQuery, new List<string>() { Constants.QueryPlan.Title });

        string modifiedQuery = string.Join(" ", query.Split(new[] { '\n', '\t' }, StringSplitOptions.RemoveEmptyEntries));
        string finalQuery = Regex.Replace(modifiedQuery, @"\s+", " ");
        
        if (!string.IsNullOrEmpty(queryPlan))
        {
            Result result = await _executionQueryHelper.ExamineQueryCachePlan(queryPlan, query);
            
            await context.RespondAsync(new GetSqlServerExecutionResponse()
            {
                ExperimentType = context.Message.ExperimentType,
                Query = finalQuery,
                IsExecutedFromCache = result.IsCached,
                QueryExecutionNumber = context.Message.QueryExecutionNumber,
                CacheHitRate = result.CacheHitRate,
                CacheMissRate = result.CacheMissRate,
                ExperimentExecutionTime = resultTime.ToString(),
                Resources = result.Resources,
                CacheSize = result.CacheSize
            });
        }
        else
        {
            await context.RespondAsync(new GetSqlServerExecutionResponse()
            {
                ExperimentType = context.Message.ExperimentType,
                Query = finalQuery,
                IsExecutedFromCache = false,
                ExperimentExecutionTime = resultTime.ToString(),
                CacheHitRate = 0,
                CacheMissRate = 100,
                Resources = ""
            });
        }
    }
}