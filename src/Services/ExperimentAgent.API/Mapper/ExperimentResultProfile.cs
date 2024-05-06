namespace ExperimentAgent.API.Mapper;

public class ExperimentResultProfile : Profile
{
    public ExperimentResultProfile()
    {
        CreateMap<GetSqlServerExecutionResponse, ExperimentResult>();
        CreateMap<GetRedisExecutionResponse, ExperimentResult>();
        CreateMap<GetMongoDbExecutionResponse, ExperimentResult>();
    }   
}