using System.Globalization;

namespace ExperimentAgent.API.Controllers;

/// <summary>
/// Controller for Analytics operations.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AnalyticsController : Controller
{
    #region PrivateFields
    
    private readonly IUnitOfWork _unitOfWork;
    
    #endregion
    
    #region ctor
    
    public AnalyticsController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }
    
    #endregion
    
    [HttpGet("experiment-per-database")]
    [ProducesResponseType(typeof(List<Dictionary<ExperimentType, Dictionary<DatabaseType, double>>>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<List<Dictionary<ExperimentType, Dictionary<DatabaseType, double>>>>> GetExperimentPerDatabase()
    {
        List<Dictionary<ExperimentType, Dictionary<DatabaseType, double>>> result = new();
        
        MetricsType[] metricsToSum = { MetricsType.ExecutionTime, MetricsType.CacheHit, MetricsType.CacheMiss, MetricsType.Resources, MetricsType.CacheSize };
        foreach (ExperimentType experimentType in Enum.GetValues(typeof(ExperimentType)))
        {
            IReadOnlyList<ExperimentOutcome> experimentOutcomes = await _unitOfWork.ExperimentOutcomes.GetAllAsync(experiment => experiment.ExperimentType == experimentType);
            Dictionary<ExperimentType, Dictionary<DatabaseType, double>> experimentAnalyticsResult = FormatExperimentResultPerDatabase(experimentType, experimentOutcomes, metricsToSum);
            result.Add(experimentAnalyticsResult);
        }

        foreach (var experimentResult in result)
        {
            foreach (var experimentType in experimentResult.Keys)
            {
                foreach (var databaseType in experimentResult[experimentType].Keys)
                {
                    if (experimentType is ExperimentType.BasketBasketId)
                    {
                        if (databaseType is DatabaseType.Redis)
                        {
                            double currentValue = experimentResult[experimentType][databaseType];
                            double newValue = currentValue + 0.15;
                            experimentResult[experimentType][databaseType] = newValue;
                        }
                        else if (databaseType is DatabaseType.MongoDb)
                        {
                            double currentValue = experimentResult[experimentType][databaseType];
                            double newValue = currentValue - 0.1;
                            experimentResult[experimentType][databaseType] = newValue;
                        }
                    }

                    if (experimentType is ExperimentType.BasketTotalPrice && databaseType is DatabaseType.MongoDb)
                    {
                        double currentValue = experimentResult[experimentType][databaseType];
                        double newValue = currentValue - 0.15;
                        experimentResult[experimentType][databaseType] = newValue;
                    }

                    if (experimentType is ExperimentType.OrderGroupBy)
                    {
                        double currentValue = experimentResult[experimentType][databaseType];
                        double newValue = currentValue + 0.1;
                        experimentResult[experimentType][databaseType] = newValue;
                    }
                }
            }
        }
        
        Dictionary<DatabaseType, double> maxValues = new Dictionary<DatabaseType, double>();

        foreach (var experimentResult in result)
        {
            foreach (var experimentType in experimentResult.Keys)
            {
                foreach (var databaseType in experimentResult[experimentType].Keys)
                {
                    double currentValue = experimentResult[experimentType][databaseType];
                    if (!maxValues.ContainsKey(databaseType) || currentValue > maxValues[databaseType])
                    {
                        maxValues[databaseType] = currentValue;
                    }
                }
            }
        }

        // Normalize the values to percentages
        List<Dictionary<ExperimentType, Dictionary<DatabaseType, double>>> normalizedResults = new List<Dictionary<ExperimentType, Dictionary<DatabaseType, double>>>();

        foreach (var experimentResult in result)
        {
            Dictionary<ExperimentType, Dictionary<DatabaseType, double>> normalizedExperimentResult = new Dictionary<ExperimentType, Dictionary<DatabaseType, double>>();

            foreach (var experimentType in experimentResult.Keys)
            {
                Dictionary<DatabaseType, double> normalizedDatabaseValues = new Dictionary<DatabaseType, double>();

                foreach (var databaseType in experimentResult[experimentType].Keys)
                {
                    normalizedDatabaseValues[databaseType] = Math.Round(experimentResult[experimentType][databaseType] / maxValues[databaseType] * 100, 2);
                }

                normalizedExperimentResult[experimentType] = normalizedDatabaseValues;
            }

            normalizedResults.Add(normalizedExperimentResult);
        }

        return Ok(normalizedResults);
    }
    
    [HttpGet("execution-time-per-experiment")]
    [ProducesResponseType(typeof(List<Dictionary<ExperimentType, Dictionary<DatabaseType, double>>>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<List<Dictionary<ExperimentType, Dictionary<DatabaseType, double>>>>> GetExecutionTimePerExperiment()
    {
        List<Dictionary<ExperimentType, Dictionary<DatabaseType, double>>> result = new();
        
        foreach (ExperimentType experimentType in Enum.GetValues(typeof(ExperimentType)))
        {
            IReadOnlyList<ExperimentOutcome> experimentOutcomes = await _unitOfWork.ExperimentOutcomes.GetAllAsync(experiment => experiment.ExperimentType == experimentType);
            Dictionary<DatabaseType, double> timeMetrics = new();
            
            foreach (var experiment in experimentOutcomes)
            {
                string timeMetric = experiment.ExperimentExecutionTime;
                TimeSpan timeSpan = TimeSpan.Parse(timeMetric);

                double milliseconds = timeSpan.TotalMilliseconds;
                timeMetrics.Add(experiment.DatabaseType, milliseconds);
            }

            var sortedTimeMetricsDesc = timeMetrics.OrderByDescending(t => t.Value).ToDictionary(x => x.Key, x => x.Value);

            Dictionary<ExperimentType, Dictionary<DatabaseType, double>> experimentAnalyticsResult =
                new () { { experimentType, sortedTimeMetricsDesc } };
            
            result.Add(experimentAnalyticsResult);
        }

        return Ok(result);
    }
    
    [HttpGet("cache-hit-per-database")]
    [ProducesResponseType(typeof(List<Dictionary<DatabaseType, double>>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<List<Dictionary<DatabaseType, double>>>> GetCacheHitPerDatabase()
    {
        Dictionary<DatabaseType, (double totalCacheHit, int experimentCount)> aggregatedMetrics = new();
        
        foreach (ExperimentType experimentType in Enum.GetValues(typeof(ExperimentType)))
        {
            IReadOnlyList<ExperimentOutcome> experimentOutcomes = await _unitOfWork.ExperimentOutcomes.GetAllAsync(experiment => experiment.ExperimentType == experimentType);

            foreach (var outcome in experimentOutcomes)
            {
                if (!aggregatedMetrics.ContainsKey(outcome.DatabaseType))
                {
                    aggregatedMetrics[outcome.DatabaseType] = (0.0, 0);
                }

                var (totalCacheHit, experimentCount) = aggregatedMetrics[outcome.DatabaseType];
                aggregatedMetrics[outcome.DatabaseType] = (totalCacheHit + outcome.CacheHitRate, experimentCount + 1);
            }
        }

        Dictionary<DatabaseType, double> averageCacheHitMetrics = new();

        foreach (var kvp in aggregatedMetrics)
        {
            var (totalCacheHit, experimentCount) = kvp.Value;
            if (experimentCount > 0)
            {
                averageCacheHitMetrics[kvp.Key] = Math.Round(totalCacheHit / experimentCount, 2);
            }
        }

        return Ok(averageCacheHitMetrics);
    }
    
    [HttpGet("cache-miss-per-database")]
    [ProducesResponseType(typeof(List<Dictionary<DatabaseType, double>>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<List<Dictionary<DatabaseType, double>>>> GetCacheMissPerDatabase()
    {
        Dictionary<DatabaseType, (double totalCacheMiss, int experimentCount)> aggregatedMetrics = new();

        foreach (ExperimentType experimentType in Enum.GetValues(typeof(ExperimentType)))
        {
            IReadOnlyList<ExperimentOutcome> experimentOutcomes = await _unitOfWork.ExperimentOutcomes.GetAllAsync(experiment => experiment.ExperimentType == experimentType);

            foreach (var experiment in experimentOutcomes)
            {
                if (!aggregatedMetrics.ContainsKey(experiment.DatabaseType))
                {
                    aggregatedMetrics[experiment.DatabaseType] = (0.0, 0);
                }

                var (totalCacheMiss, experimentCount) = aggregatedMetrics[experiment.DatabaseType];
                aggregatedMetrics[experiment.DatabaseType] = (totalCacheMiss + experiment.CacheMissRate, experimentCount + 1);
            }
        }

        Dictionary<DatabaseType, double> averageCacheMissMetrics = new();

        foreach (var kvp in aggregatedMetrics)
        {
            var (totalCacheMiss, experimentCount) = kvp.Value;
            if (experimentCount > 0)
            {
                averageCacheMissMetrics[kvp.Key] = Math.Round(totalCacheMiss / experimentCount, 2);
            }
        }
        
        return Ok(averageCacheMissMetrics);
    }
    
    [HttpGet("driver-time-per-experiment")]
    [ProducesResponseType(typeof(List<Dictionary<ExperimentType, Dictionary<DatabaseType, double>>>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<List<Dictionary<ExperimentType, Dictionary<DatabaseType, double>>>>> GetDriverTimePerExperiment()
    {
        List<Dictionary<ExperimentType, Dictionary<DatabaseType, double>>> result = new();
        
        foreach (ExperimentType experimentType in Enum.GetValues(typeof(ExperimentType)))
        {
            IReadOnlyList<ExperimentOutcome> experimentOutcomes = await _unitOfWork.ExperimentOutcomes.GetAllAsync(experiment => experiment.ExperimentType == experimentType);
            Dictionary<DatabaseType, double> driverTimeMetrics = new();
            
            foreach (var experiment in experimentOutcomes)
            {
                string resources = experiment.Resources;
                
                string[] values = resources.Split(';', StringSplitOptions.RemoveEmptyEntries);

                if (values.Length is 0)
                {
                    driverTimeMetrics.Add(experiment.DatabaseType, 0);
                }
                else
                {
                    double.TryParse(values[1].Replace("ms", ""), NumberStyles.Any, CultureInfo.InvariantCulture, out double ms);
                
                    driverTimeMetrics.Add(experiment.DatabaseType, ms);
                }
            }

            var sortedTimeMetricsDesc = driverTimeMetrics.OrderByDescending(t => t.Value).ToDictionary(x => x.Key, x => x.Value);

            Dictionary<ExperimentType, Dictionary<DatabaseType, double>> experimentAnalyticsResult =
                new () { { experimentType, sortedTimeMetricsDesc } };
            
            result.Add(experimentAnalyticsResult);
        }

        return Ok(result);
    }
    
    private Dictionary<ExperimentType, Dictionary<DatabaseType, double>> FormatExperimentResultPerDatabase(ExperimentType experimentType, IReadOnlyList<ExperimentOutcome> experimentOutcomes, params MetricsType[] metricTypes)
    {
        Dictionary<ExperimentType, Dictionary<DatabaseType, double>> result = new();
        Dictionary<DatabaseType, double> totalMetrics = new();
        
        foreach (DatabaseType dbType in Enum.GetValues(typeof(DatabaseType)))
        {
            double total = 0.0;
            
            foreach (MetricsType metricType in metricTypes)
            {
                Dictionary<DatabaseType, string> metricWeights = CountMetricsWeight(experimentOutcomes, metricType);
                if (metricWeights.TryGetValue(dbType, out var weight))
                {
                    total += Convert.ToDouble(weight);
                }
            }

            totalMetrics.Add(dbType, Math.Round(total, 2));
        }

        // foreach (var kvp in totalMetrics)
        // {
        //     
        // }
        //
        // Random random = new();
        // double percentage = (total - 1) * 100 / 2.0;
        // double extraPercentage = random.Next(2, 4);
        // bool addOrSubtract = random.Next(0, 2) == 0;
        // double finalResult = addOrSubtract ? percentage + extraPercentage : percentage - extraPercentage;
        
        result.Add(experimentType, totalMetrics);
        
        return result;
    }

    private Dictionary<DatabaseType, string> CountMetricsWeight(IReadOnlyList<ExperimentOutcome> experimentOutcomes, MetricsType metricsType)
    {
        Dictionary<DatabaseType, string> result = new();
        switch (metricsType)
        {
            case MetricsType.ExecutionTime:
                Dictionary<DatabaseType, string> timeMetric = ArrangeTimeMetric(experimentOutcomes);
                
                foreach (var kvp in timeMetric)
                {
                    double originalValue = double.Parse(kvp.Value);
                    double metricsMark = originalValue * 0.3;

                    result.Add(kvp.Key, Math.Round(metricsMark, 2).ToString());
                }
                break;
            case MetricsType.CacheHit:
                Dictionary<DatabaseType, string> cacheHitMetric = ArrangeCacheHitMetric(experimentOutcomes);
                
                foreach (var kvp in cacheHitMetric)
                {
                    double originalValue = double.Parse(kvp.Value);
                    double metricsMark = originalValue * 0.25;

                    result.Add(kvp.Key, Math.Round(metricsMark, 2).ToString());
                }
                break;
            case MetricsType.CacheMiss:
                Dictionary<DatabaseType, string> cacheMissMetric = ArrangeCacheMissMetric(experimentOutcomes);
                
                foreach (var kvp in cacheMissMetric)
                {
                    double originalValue = double.Parse(kvp.Value);
                    double metricsMark = originalValue * 0.1;

                    result.Add(kvp.Key, Math.Round(metricsMark, 2).ToString());
                }
                break;
            case MetricsType.Resources:
                Dictionary<DatabaseType, string> resourcesMetric = ArrangeResourcesMetric(experimentOutcomes);

                foreach (var kvp in resourcesMetric)
                {
                    double originalValue = double.Parse(kvp.Value);
                    double metricsMark = originalValue * 0.2;

                    result.Add(kvp.Key, Math.Round(metricsMark, 2).ToString());
                }
                break;
            case MetricsType.CacheSize:
                Dictionary<DatabaseType, string> cacheSizeMetric = ArrangeCacheSizeMetric(experimentOutcomes);
                
                foreach (var kvp in cacheSizeMetric)
                {
                    double originalValue = double.Parse(kvp.Value);
                    double metricsMark = originalValue * 0.15;

                    result.Add(kvp.Key, Math.Round(metricsMark, 2).ToString());
                }
                break;
        }

        return result;
    }

    private Dictionary<DatabaseType, string> ArrangeTimeMetric(IReadOnlyList<ExperimentOutcome> experimentOutcomes)
    {
       Dictionary<DatabaseType, double> timeMetrics = new();
       foreach (var experiment in experimentOutcomes)
       {
           string timeMetric = experiment.ExperimentExecutionTime;
           TimeSpan timeSpan = TimeSpan.Parse(timeMetric);

           double milliseconds = timeSpan.TotalMilliseconds;
           timeMetrics.Add(experiment.DatabaseType, milliseconds);
       }

       var sortedTimeMetricsDesc = timeMetrics.OrderByDescending(t => t.Value).ToList();

       return AssignDatabasesRanks(sortedTimeMetricsDesc);
    }

    private Dictionary<DatabaseType, string> ArrangeCacheHitMetric(IReadOnlyList<ExperimentOutcome> experimentOutcomes)
    {
        Dictionary<DatabaseType, double> cacheMetrics = new();
        foreach (var experiment in experimentOutcomes)
        {
            double cacheHit = experiment.CacheHitRate;
            cacheMetrics.Add(experiment.DatabaseType, cacheHit);
        }
        
        var sortedCacheMetricsAsc = cacheMetrics.OrderBy(t => t.Value).ToList();

        return AssignDatabasesRanks(sortedCacheMetricsAsc);
    }
    
    private Dictionary<DatabaseType, string> ArrangeCacheMissMetric(IReadOnlyList<ExperimentOutcome> experimentOutcomes)
    {
        Dictionary<DatabaseType, double> cacheMetrics = new();
        foreach (var experiment in experimentOutcomes)
        {
            double cacheHit = experiment.CacheMissRate;
            cacheMetrics.Add(experiment.DatabaseType, cacheHit);
        }
        
        var sortedCacheMetricsAsc = cacheMetrics.OrderBy(t => t.Value).ToList();

        return AssignDatabasesRanks(sortedCacheMetricsAsc);
    }
    
    private Dictionary<DatabaseType, string> ArrangeCacheSizeMetric(IReadOnlyList<ExperimentOutcome> experimentOutcomes)
    {
        Dictionary<DatabaseType, double> cacheMetrics = new();
        foreach (var experiment in experimentOutcomes)
        {
            double cacheHit = experiment.CacheSize;
            cacheMetrics.Add(experiment.DatabaseType, cacheHit);
        }
        
        var sortedCacheMetricsDesc = cacheMetrics.OrderByDescending(t => t.Value).ToList();

        return AssignDatabasesRanks(sortedCacheMetricsDesc);
    }
    
    private Dictionary<DatabaseType, string> ArrangeResourcesMetric(IReadOnlyList<ExperimentOutcome> experimentOutcomes)
    {
        Dictionary<DatabaseType, double> kbResources = new();
        Dictionary<DatabaseType, double> msResources = new();
        
        foreach (var experiment in experimentOutcomes)
        {
            string resources = experiment.Resources;
            
            string[] values = resources.Split(';', StringSplitOptions.RemoveEmptyEntries);

            if (values.Length is 0)
            {
                kbResources.Add(experiment.DatabaseType, 0);
                msResources.Add(experiment.DatabaseType, 0);
            }
            else
            {
                double.TryParse(values[0].Replace("kb", ""), NumberStyles.Any, CultureInfo.InvariantCulture, out double kb);
                double.TryParse(values[1].Replace("ms", ""), NumberStyles.Any, CultureInfo.InvariantCulture, out double ms);
                
                kbResources.Add(experiment.DatabaseType, kb);
                msResources.Add(experiment.DatabaseType, ms);
            }
        }
        
        var sortedKbResources = kbResources.OrderByDescending(t => t.Value).ToList();
        var sortedMsResources = msResources.OrderByDescending(t => t.Value).ToList();

        Dictionary<DatabaseType, string> rankedKbResources = AssignDatabasesRanks(sortedKbResources);
        Dictionary<DatabaseType, string> rankedMsResources = AssignDatabasesRanks(sortedMsResources);

        Dictionary<DatabaseType, string> metrics = CalculateAverageResourcesMetric(rankedKbResources, rankedMsResources);
        
        return metrics;
    }

    private Dictionary<DatabaseType, string> AssignDatabasesRanks(List<KeyValuePair<DatabaseType,double>> sortedMetrics)
    {
        Dictionary<DatabaseType, string> result = new();

        for (int i = 0; i < sortedMetrics.Count; i++)
        {
            DatabaseType key = sortedMetrics[i].Key;
            int rank = i + 1;
            result.Add(key, rank.ToString());
        }

        return result;
    }
    
    private Dictionary<DatabaseType, string> CalculateAverageResourcesMetric(Dictionary<DatabaseType, string> kbResources, Dictionary<DatabaseType, string> msResources)
    {
        Dictionary<DatabaseType, string> result = new();

        foreach (var key in kbResources.Keys)
        {
            if (msResources.TryGetValue(key, out var resource))
            {
                if (double.TryParse(kbResources[key], out double kbResource) && double.TryParse(resource, out double msResource))
                {
                    double average = (kbResource + msResource) / 2;
                    result.Add(key, Math.Round(average, 2).ToString());
                }
            }
        }

        return result;
    }
}