namespace SQLServerAgent.API.Entities;

public enum OrderStatus
{
    Submitted,
    AwaitingValidation,
    Confirmed,
    Paid,
    Shipped,
    Cancelled
}