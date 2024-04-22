namespace MongoAgent.API.Common;

public record Result(bool IsExecutedFromCache, string Resources, string Pipeline);