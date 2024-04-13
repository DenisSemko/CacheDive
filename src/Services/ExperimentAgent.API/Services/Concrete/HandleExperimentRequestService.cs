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
                GetSqlServerExecutionRequest executionRequestEvent = new() { ExperimentType = request.ExperimentType, QueryExecutionNumber = request.QueryExecutionNumber, IsCacheCleaned = request.IsCacheCleaned };
                
                Response<GetSqlServerExecutionResponse> sqlServerResponse = await _sqlRequestClient.GetResponse<GetSqlServerExecutionResponse>(executionRequestEvent, cancellationToken);
                experimentResult = _mapper.Map<ExperimentResult>(sqlServerResponse.Message);

                await ManageInternalDatabase(request, experimentResult);
                break;
            case DatabaseType.Redis:
                GetRedisExecutionRequest executionRedisRequest = new() { ExperimentType = request.ExperimentType, QueryExecutionNumber = request.QueryExecutionNumber };
                
                Response<GetRedisExecutionResponse> redisResponse = await _redisRequestClient.GetResponse<GetRedisExecutionResponse>(executionRedisRequest, cancellationToken);
                experimentResult = _mapper.Map<ExperimentResult>(redisResponse.Message);
                
                await ManageInternalDatabase(request, experimentResult);
                break;
            case DatabaseType.MongoDb:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        return experimentResult;
    }

    private async Task ManageInternalDatabase(ExperimentRequest request, ExperimentResult experimentResult)
    {
        ExperimentOutcome outcome = new()
        {
            Id = Guid.NewGuid(),
            Query = experimentResult.Query,
            DatabaseType = request.DatabaseType,
            IsExecutedFromCache = experimentResult.IsExecutedFromCache,
            QueryExecutionNumber = experimentResult.QueryExecutionNumber,
            QueryExecutionTime = experimentResult.QueryExecutionTime
        };

        if (experimentResult.IsExecutedFromCache)
        {
            outcome.CacheHitRate = experimentResult.CacheHitRate;
            outcome.CacheMissRate = experimentResult.CacheMissRate;
            outcome.Resources = experimentResult.Resources;
            outcome.CacheSize = experimentResult.CacheSize;
        }
        
        await _unitOfWork.ExperimentOutcomes.InsertOneAsync(outcome);
    }
}