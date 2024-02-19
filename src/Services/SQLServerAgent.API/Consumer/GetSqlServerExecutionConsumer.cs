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
        string query = context.Message.Query;
        
        //clear cache
        await _executionQueryHelper.ExecuteQuery(Constants.Queries.ClearCacheQuery);
        
        //main query execution
        TimeSpan resultTime = await _executionTimeHelper.MeasureExecutionTime(async () =>
        {
            await _executionQueryHelper.ExecuteQuery(query);
        }, context.Message.QueryExecutionNumber);
        
        // execution plan
        string updatedQuery = string.Format(Constants.Queries.GetExecutionPlanQuery, query.Replace("'", "''"));

        string queryPlan = await _executionQueryHelper.ExecuteQueryWithReader(updatedQuery, new List<string>() { Constants.QueryPlan.Title });

        if (!string.IsNullOrEmpty(queryPlan))
        {
            Result result = await _executionQueryHelper.ExamineQueryCachePlan(queryPlan, query);

            await context.RespondAsync(new GetSqlServerExecutionResponse()
            {
                IsExecutedFromCache = result.IsCached,
                QueryExecutionNumber = context.Message.QueryExecutionNumber,
                CacheHitRate = result.CacheHitRate,
                CacheMissRate = result.CacheMissRate,
                QueryExecutionTime = resultTime.ToString(),
                Resources = result.Resources,
                CacheSize = result.CacheSize
            });
        }
        else
        {
            await context.RespondAsync(new GetSqlServerExecutionResponse()
            {
                IsExecutedFromCache = false
            });
        }
    }
}