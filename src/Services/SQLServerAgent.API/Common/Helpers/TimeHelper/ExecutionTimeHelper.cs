namespace SQLServerAgent.API.Common.Helpers.TimeHelper;

public class ExecutionTimeHelper : IExecutionTimeHelper
{
    public async Task<TimeSpan> MeasureExecutionTime(Func<Task> action, int executionNumber)
    {
        List<TimeSpan> executionTimes = new List<TimeSpan>();

        for (int i = 0; i < executionNumber; i++)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            await action(); 

            stopwatch.Stop();
            executionTimes.Add(stopwatch.Elapsed);
        }

        executionTimes.Sort();
        return executionTimes[executionTimes.Count / 2];
    }
}