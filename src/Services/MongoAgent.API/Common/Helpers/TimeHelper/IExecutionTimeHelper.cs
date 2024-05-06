namespace MongoAgent.API.Common.Helpers.TimeHelper;

public interface IExecutionTimeHelper
{
    Task<TimeSpan> MeasureExecutionTime(Func<Task> action, int executionNumber);
}