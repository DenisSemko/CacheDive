namespace RedisAgent.API.Entities;

public enum OrderStatus
{
    Submitted = 1,
    AwaitingValidation,
    Confirmed,
    Paid,
    Shipped,
    Cancelled
}