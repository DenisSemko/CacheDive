using EventBus.Messages.Common;

namespace SQLServerAgent.API.Common.Helpers.QueryHelper;

public interface IExecutionQueryHelper
{
    Task ExecuteQuery(string query);
    Task<string> ExecuteQueryWithReader(string query, List<string> readerRows);
    Task<Result> ExamineQueryCachePlan(string queryPlan, string originalQuery);
    string HandleExperimentType(ExperimentType experimentType);
}