namespace SQLServerAgent.API.Common;

public record Result(bool IsCached, double CacheHitRate, double CacheMissRate, string Resources, double CacheSize);