namespace MongoAgent.API.Entities;

[BsonCollection("ProductOrder")]
public class ProductOrder
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; set; }
    public int Quantity { get; set; }
    public double Price { get; set; }
    public Guid ProductId { get; set; }
    public Guid OrderId { get; set; }
}