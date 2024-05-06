namespace MongoAgent.API.Entities;

[BsonCollection("Order")]
public class Order
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; set; }
    public double TotalPrice { get; set; }
    public DateTime OrderDate { get; set; }
    public OrderStatus OrderStatus { get; set; }
    public ShippingType ShippingType { get; set; }
    public PaymentType PaymentType { get; set; }
    public string ShippingAddress { get; set; }
    [BsonRepresentation(BsonType.String)]
    public Guid UserId { get; set; }
}