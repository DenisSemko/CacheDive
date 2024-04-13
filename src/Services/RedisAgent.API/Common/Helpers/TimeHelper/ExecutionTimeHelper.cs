using System.Diagnostics;

namespace RedisAgent.API.Common.Helpers.TimeHelper;

public class ExecutionTimeHelper : IExecutionTimeHelper
{
    public async Task<TimeSpan> MeasureExecutionTime(Func<Task> action)
    {
        List<TimeSpan> executionTimes = new List<TimeSpan>();

        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        await action(); 

        stopwatch.Stop();
        
        executionTimes.Add(stopwatch.Elapsed);
        return executionTimes[executionTimes.Count - 1];
    }
}