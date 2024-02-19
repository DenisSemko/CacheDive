namespace SQLServerAgent.API.Common.Helpers.QueryHelper;

public class ExecutionQueryHelper : IExecutionQueryHelper
{
    private readonly IDatabaseConfiguration _databaseConfiguration;

    public ExecutionQueryHelper(IDatabaseConfiguration databaseConfiguration)
    {
        _databaseConfiguration = databaseConfiguration;
    }
    
    public async Task ExecuteQuery(string query)
    {
        await using (SqlConnection connection = new SqlConnection(_databaseConfiguration.DefaultConnection))
        {
            connection.Open();

            await using (SqlCommand sqlCommand = new SqlCommand(query, connection))
            {
                await sqlCommand.ExecuteNonQueryAsync();
            }
        }
    }

    public async Task<string> ExecuteQueryWithReader(string query, List<string> readerRows)
    {
        string result = string.Empty;
        
        await using (SqlConnection connection = new SqlConnection(_databaseConfiguration.DefaultConnection))
        {
            connection.Open();

            await using (SqlCommand sqlCommand = new SqlCommand(query, connection))
            {
                await using (SqlDataReader dataReader = await sqlCommand.ExecuteReaderAsync())
                {
                    while (dataReader.Read())
                    {
                        foreach (string row in readerRows)
                        {
                            result += dataReader[row] + ";";
                        }
                    }
                }
            }
        }

        return result;
    }
    
    //refactor
    public async Task<Result> ExamineQueryCachePlan(string queryPlan, string originalQuery)
    {
        bool isExecutedFromCache = queryPlan.Contains(Constants.QueryPlan.RetrievedFromCache);
        
        //cache hint + miss
        string updatedQuery = originalQuery.Replace("'", "''");
        string cacheHitMissQuery = string.Format(Constants.Queries.GetCacheHitMissQuery, updatedQuery);
        string cacheHintMissRates = await ExecuteQueryWithReader(cacheHitMissQuery, new List<string>() { Constants.QueryPlan.CacheHitColumnName, Constants.QueryPlan.CacheMissColumnName });
        string[] rates = cacheHintMissRates.Split(';');
        double cacheHitRate = double.Parse(rates[0].Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture);
        double cacheMissRate = double.Parse(rates[1].Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture);
    
        //resources
        int startIndex = queryPlan.IndexOf(Constants.QueryPlan.QueryPlanTag, StringComparison.Ordinal);
        int endIndex = queryPlan.IndexOf(">", startIndex, StringComparison.Ordinal);
        string extractedQueryPlanTag = queryPlan.Substring(startIndex, endIndex - startIndex + 1);
        
        //CachedPlanSize - the size of the cached query in kilobytes
        //CompileCPU - the CPU time, in milliseconds, used for query
        //CompileMemory - the amount of memory used for the query (in kilobytes)
        int cachedPlanSize = ExtractValue(extractedQueryPlanTag, Constants.QueryPlan.CachedPlanSize);
        int compileCPU = ExtractValue(extractedQueryPlanTag, Constants.QueryPlan.CompileCPU);
        int compileMemory = ExtractValue(extractedQueryPlanTag, Constants.QueryPlan.CompileMemory);
        string resources = cachedPlanSize + "kb;" + compileCPU + "ms;" + compileMemory + "kb;";
        
        //execute query to get cache size 
        string cacheSize = await ExecuteQueryWithReader(Constants.Queries.GetCacheSizeQuery, new List<string>() { Constants.QueryPlan.ProcedureCacheColumnName });
        
        return new Result(isExecutedFromCache, cacheHitRate, cacheMissRate, resources, int.Parse(cacheSize.Replace(";", "")));
    }
    
    private int ExtractValue(string input, string attributeName)
    {
        string pattern = $@"{attributeName}=""(\d+)""";
        Match match = Regex.Match(input, pattern);

        return match.Success && match.Groups.Count > 1
            ? int.Parse(match.Groups[1].Value)
            : -1;
    }
}