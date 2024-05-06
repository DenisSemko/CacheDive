namespace RedisAgent.API.Consumer;

public class GetRedisExecutionConsumer : IConsumer<GetRedisExecutionRequest>
{
    private readonly IExecutionQueryHelper _executionQueryHelper;
    private readonly IExecutionTimeHelper _executionTimeHelper;

    public GetRedisExecutionConsumer(IExecutionQueryHelper executionQueryHelper, IExecutionTimeHelper executionTimeHelper)
    {
        _executionQueryHelper = executionQueryHelper ?? throw new ArgumentNullException(nameof(executionQueryHelper));
        _executionTimeHelper = executionTimeHelper ?? throw new ArgumentNullException(nameof(executionTimeHelper));
    }

    public async Task Consume(ConsumeContext<GetRedisExecutionRequest> context)
    {
        TimeSpan resultTime = TimeSpan.Zero;
        double cacheHitRate = default;
        double cacheMissRate = default;
        double keyMemoryUsage = default;
        string resources = string.Empty;
        
        switch (context.Message.ExperimentType)
        {
            case ExperimentType.BasketBasketId:
                List<dynamic> basketFirstQueryResult = new();
                resultTime = await _executionTimeHelper.MeasureExecutionTime(async () =>
                {
                    basketFirstQueryResult = await _executionQueryHelper.GetProductsFromBasketByBasketId();
                });
                
                if (basketFirstQueryResult.Any())
                {
                    cacheHitRate = 100;
                    cacheMissRate = 0;
                    foreach (var anonObj in basketFirstQueryResult)
                    {
                        Type type = anonObj.GetType();
                        PropertyInfo? keysMemoryUsage = type.GetProperty("KeysMemoryUsage");
                        PropertyInfo? executionTime = type.GetProperty("ExecutionTime");

                        if (keysMemoryUsage is not null)
                        {
                            double value = keysMemoryUsage.GetValue(anonObj);
                            resources =  Math.Round(value, 2) + "kb;";
                            resources = resources.Replace(',', '.');
                        }

                        if (executionTime is not null)
                        {
                            TimeSpan value = executionTime.GetValue(anonObj);
                            double milliseconds = value.TotalMilliseconds;
                            string millisecondsStr = Math.Round(milliseconds, 2).ToString().Replace(',', '.');
                            resources +=  $"{millisecondsStr}ms;";
                        }
                    }
                }
                else
                {
                    cacheHitRate = 0;
                    cacheMissRate = 100;
                }
                break;
            case ExperimentType.BasketUserId:
                List<dynamic> basketSecondQueryResult = new();
                resultTime = await _executionTimeHelper.MeasureExecutionTime(async () =>
                {
                    basketSecondQueryResult = await _executionQueryHelper.GetProductsFromBasketWithUserDetails();
                });
                
                if (basketSecondQueryResult.Any())
                {
                    cacheHitRate = 100;
                    cacheMissRate = 0;
                    
                    foreach (var anonObj in basketSecondQueryResult)
                    {
                        Type type = anonObj.GetType();
                        PropertyInfo? keysMemoryUsage = type.GetProperty("KeysMemoryUsage");
                        PropertyInfo? executionTime = type.GetProperty("ExecutionTime");

                        if (keysMemoryUsage is not null)
                        {
                            double value = keysMemoryUsage.GetValue(anonObj);
                            resources =  Math.Round(value, 3) + "kb;";
                            resources = resources.Replace(',', '.');
                        }
                        
                        if (executionTime is not null)
                        {
                            TimeSpan value = executionTime.GetValue(anonObj);
                            double milliseconds = value.TotalMilliseconds;
                            string millisecondsStr = Math.Round(milliseconds, 2).ToString().Replace(',', '.');
                            resources +=  $"{millisecondsStr}ms;";
                        }
                    }
                }
                else
                {
                    cacheHitRate = 0;
                    cacheMissRate = 100;
                }
                break;
            case ExperimentType.BasketTotalPrice:
                dynamic basketThirdQueryResult = default!;
                resultTime = await _executionTimeHelper.MeasureExecutionTime(async () =>
                {
                    basketThirdQueryResult = await _executionQueryHelper.CountTotalPriceQuantityByBasketId();
                });
                
                if (basketThirdQueryResult.TotalSum is not null)
                {
                    cacheHitRate = 100;
                    cacheMissRate = 0;

                    dynamic? keysMemoryUsage = basketThirdQueryResult.KeysMemoryUsage;
                    dynamic? executionTime = basketThirdQueryResult.ExecutionTime;
                    
                    if (keysMemoryUsage is not null)
                    {
                        resources =  Math.Round(keysMemoryUsage, 3) + "kb;";
                        resources = resources.Replace(',', '.');
                    }
                    
                    if (executionTime is not null)
                    {
                        double milliseconds = executionTime.TotalMilliseconds;
                        string millisecondsStr = Math.Round(milliseconds, 2).ToString().Replace(',', '.');
                        resources +=  $"{millisecondsStr}ms;";
                    }
                }
                else
                {
                    cacheHitRate = 0;
                    cacheMissRate = 100;
                }
                break;
            case ExperimentType.ProductSmartphones:
                List<dynamic> productFirstQueryResult = new();
                resultTime = await _executionTimeHelper.MeasureExecutionTime(async () =>
                {
                    productFirstQueryResult = await _executionQueryHelper.GetProductsByCategory();
                });

                if (productFirstQueryResult.Any())
                {
                    cacheHitRate = 100;
                    cacheMissRate = 0;
                    
                    foreach (var anonObj in productFirstQueryResult)
                    {
                        Type type = anonObj.GetType();
                        PropertyInfo? keysMemoryUsage = type.GetProperty("KeysMemoryUsage");
                        PropertyInfo? executionTime = type.GetProperty("ExecutionTime");

                        if (keysMemoryUsage is not null)
                        {
                            double value = keysMemoryUsage.GetValue(anonObj);
                            resources =  Math.Round(value, 3) + "kb;";
                            resources = resources.Replace(',', '.');
                        }
                        
                        if (executionTime is not null)
                        {
                            TimeSpan value = executionTime.GetValue(anonObj);
                            double milliseconds = value.TotalMilliseconds;
                            string millisecondsStr = Math.Round(milliseconds, 2).ToString().Replace(',', '.');
                            resources +=  $"{millisecondsStr}ms;";
                        }
                    }
                }
                else
                {
                    cacheHitRate = 0;
                    cacheMissRate = 100;
                }
                break;
            case ExperimentType.ProductPrice:
                List<dynamic> productSecondQueryResult = new();
                resultTime = await _executionTimeHelper.MeasureExecutionTime(async () =>
                {
                    productSecondQueryResult = await _executionQueryHelper.GetProductsByPrice();
                });

                if (productSecondQueryResult.Any())
                {
                    cacheHitRate = 100;
                    cacheMissRate = 0;
                    
                    foreach (var anonObj in productSecondQueryResult)
                    {
                        Type type = anonObj.GetType();
                        PropertyInfo? keysMemoryUsage = type.GetProperty("KeysMemoryUsage");
                        PropertyInfo? executionTime = type.GetProperty("ExecutionTime");

                        if (keysMemoryUsage is not null)
                        {
                            double value = keysMemoryUsage.GetValue(anonObj);
                            resources =  Math.Round(value, 3) + "kb;";
                            resources = resources.Replace(',', '.');
                        }

                        if (executionTime is not null)
                        {
                            TimeSpan value = executionTime.GetValue(anonObj);
                            double milliseconds = value.TotalMilliseconds;
                            string millisecondsStr = Math.Round(milliseconds, 2).ToString().Replace(',', '.');
                            resources +=  $"{millisecondsStr}ms;";
                        }
                    }
                }
                else
                {
                    cacheHitRate = 0;
                    cacheMissRate = 100;
                }
                break;
            case ExperimentType.ProductLike:
                List<dynamic> productThirdQueryResult = new();
                resultTime = await _executionTimeHelper.MeasureExecutionTime(async () =>
                {
                    productThirdQueryResult = await _executionQueryHelper.GetProductsWithLikeFilter();
                });

                if (productThirdQueryResult.Any())
                {
                    cacheHitRate = 100;
                    cacheMissRate = 0;
                    
                    foreach (var anonObj in productThirdQueryResult)
                    {
                        Type type = anonObj.GetType();
                        PropertyInfo? keysMemoryUsage = type.GetProperty("KeysMemoryUsage");
                        PropertyInfo? executionTime = type.GetProperty("ExecutionTime");

                        if (keysMemoryUsage is not null)
                        {
                            double value = keysMemoryUsage.GetValue(anonObj);
                            resources =  Math.Round(value, 3) + "kb;";
                            resources = resources.Replace(',', '.');
                        }
                        
                        if (executionTime is not null)
                        {
                            TimeSpan value = executionTime.GetValue(anonObj);
                            double milliseconds = value.TotalMilliseconds;
                            string millisecondsStr = Math.Round(milliseconds, 2).ToString().Replace(',', '.');
                            resources +=  $"{millisecondsStr}ms;";
                        }
                    }
                }
                else
                {
                    cacheHitRate = 0;
                    cacheMissRate = 100;
                }
                break;
            case ExperimentType.OrderGroupBy:
                List<dynamic> orderFirstQueryResult = new();
                resultTime = await _executionTimeHelper.MeasureExecutionTime(async () =>
                {
                    orderFirstQueryResult = await _executionQueryHelper.CountTotalSpentForEachUser();
                });

                if (orderFirstQueryResult.Any())
                {
                    cacheHitRate = 100;
                    cacheMissRate = 0;
                    
                    foreach (var anonObj in orderFirstQueryResult)
                    {
                        Type type = anonObj.GetType();
                        PropertyInfo? keysMemoryUsage = type.GetProperty("KeysMemoryUsage");
                        PropertyInfo? executionTime = type.GetProperty("ExecutionTime");

                        if (keysMemoryUsage is not null)
                        {
                            double value = keysMemoryUsage.GetValue(anonObj);
                            resources =  Math.Round(value, 3) + "kb;";
                            resources = resources.Replace(',', '.');
                        }
                        
                        if (executionTime is not null)
                        {
                            TimeSpan value = executionTime.GetValue(anonObj);
                            double milliseconds = value.TotalMilliseconds;
                            string millisecondsStr = Math.Round(milliseconds, 2).ToString().Replace(',', '.');
                            resources +=  $"{millisecondsStr}ms;";
                        }
                    }
                }
                else
                {
                    cacheHitRate = 0;
                    cacheMissRate = 100;
                }
                break;
        }

        double cacheSize = _executionQueryHelper.GetRedisCacheSize();
        
        await context.RespondAsync(new GetRedisExecutionResponse()
        {
            ExperimentType = context.Message.ExperimentType,
            QueryExecutionNumber = context.Message.QueryExecutionNumber,
            CacheHitRate = cacheHitRate,
            CacheMissRate = cacheMissRate,
            ExperimentExecutionTime = resultTime.ToString(),
            Resources = resources,
            CacheSize = cacheSize
        });
    }
}