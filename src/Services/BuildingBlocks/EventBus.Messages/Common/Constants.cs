namespace EventBus.Messages.Common;

public static class Constants
{
    public const string TransferSqlJsonQueue = "transfer-sql-json-queue";
    public const string TransferRedisJsonQueue = "transfer-redis-json-queue";
    public const string TransferMongoJsonQueue = "transfer-mongo-json-queue";
    public const string GetSqlServerExecutionQueue = "sql-server-execution-queue";
    public const string GetRedisExecutionQueue = "redis-execution-queue";
    public const string GetMongoDbExecutionQueue = "mongodb-execution-queue";
}