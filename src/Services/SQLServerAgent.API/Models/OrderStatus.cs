namespace SQLServerAgent.API.Models;

public enum OrderStatus
{
    Submitted,
    AwaitingValidation,
    Confirmed,
    Paid,
    Shipped,
    Cancelled
}