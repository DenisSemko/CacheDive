namespace RedisAgent.API.Common.Helpers.TimeHelper;

public interface IExecutionTimeHelper
{
    Task<TimeSpan> MeasureExecutionTime(Func<Task> action);
}