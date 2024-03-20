namespace RedisAgent.API.Entities;

public class Order
{
    public Guid Id { get; set; }
    public double TotalPrice { get; set; }
    public DateTime OrderDate { get; set; }
    public OrderStatus OrderStatus { get; set; }
    public ShippingType ShippingType { get; set; }
    public PaymentType PaymentType { get; set; }
    public string ShippingAddress { get; set; }
    public Guid UserId { get; set; }
}