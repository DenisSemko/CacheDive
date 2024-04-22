namespace MongoAgent.API.Entities;

[BsonCollection("ProductOrder")]
public class ProductOrder
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; set; }
    public int Quantity { get; set; }
    public double Price { get; set; }
    [BsonRepresentation(BsonType.String)]
    public Guid ProductId { get; set; }
    [BsonRepresentation(BsonType.String)]
    public Guid OrderId { get; set; }
}