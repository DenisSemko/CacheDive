namespace MongoAgent.API.Common;

public record CacheStats(double CacheHitRate, double CacheMissRate, double CacheSize);