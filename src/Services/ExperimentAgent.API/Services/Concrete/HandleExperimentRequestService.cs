namespace ExperimentAgent.API.Services.Concrete;

public class HandleExperimentRequestService : IHandleExperimentRequestService
{
    private readonly IRequestClient<GetSqlServerExecutionRequest> _sqlRequestClient;
    private readonly IRequestClient<GetRedisExecutionRequest> _redisRequestClient;
    private readonly IRequestClient<GetMemcachedExecutionRequest> _memcachedRequestClient;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public HandleExperimentRequestService(IRequestClient<GetSqlServerExecutionRequest> sqlRequestClient, IRequestClient<GetRedisExecutionRequest> redisRequestClient, IRequestClient<GetMemcachedExecutionRequest> memcachedRequestClient, IMapper mapper, IUnitOfWork unitOfWork)
    {
        _sqlRequestClient = sqlRequestClient ?? throw new ArgumentNullException(nameof(sqlRequestClient));
        _redisRequestClient = redisRequestClient ?? throw new ArgumentNullException(nameof(redisRequestClient));
        _memcachedRequestClient = memcachedRequestClient ?? throw new ArgumentNullException(nameof(memcachedRequestClient));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }
    
    public async Task<ExperimentResult> HandleRequest(ExperimentRequest request, CancellationToken cancellationToken = default)
    {
        ExperimentResult experimentResult = new();
        
        switch (request.DatabaseType)
        {
            case DatabaseType.MSSQL:
                GetSqlServerExecutionRequest executionRequestEvent = new() { Query = request.Query, QueryExecutionNumber = request.QueryExecutionNumber, IsCacheCleaned = request.IsCacheCleaned };
                
                Response<GetSqlServerExecutionResponse> sqlServerResponse = await _sqlRequestClient.GetResponse<GetSqlServerExecutionResponse>(executionRequestEvent, cancellationToken);
                experimentResult = _mapper.Map<ExperimentResult>(sqlServerResponse.Message);

                await ManageInternalDatabase(request, experimentResult);
                break;
            case DatabaseType.Redis:
                break;
            case DatabaseType.Memcached:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        return experimentResult;
    }

    private async Task ManageInternalDatabase(ExperimentRequest request, ExperimentResult experimentResult)
    {
        if (experimentResult.IsExecutedFromCache)
        {
            ExperimentOutcome outcome = new()
            {
                Id = Guid.NewGuid(),
                Query = request.Query,
                DatabaseType = request.DatabaseType,
                QueryExecutionNumber = experimentResult.QueryExecutionNumber,
                CacheHitRate = experimentResult.CacheHitRate,
                CacheMissRate = experimentResult.CacheMissRate,
                QueryExecutionTime = experimentResult.QueryExecutionTime,
                Resources = experimentResult.Resources,
                CacheSize = experimentResult.CacheSize
            };

            await _unitOfWork.ExperimentOutcomes.InsertOneAsync(outcome);
        }
    }
}