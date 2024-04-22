namespace MongoAgent.API.Entities;

[BsonCollection("ProductBasket")]
public class ProductBasket
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; set; }
    public int Quantity { get; set; }
    public double Price { get; set; }
    [BsonRepresentation(BsonType.String)]
    public Guid ProductId { get; set; }
    [BsonRepresentation(BsonType.String)]
    public Guid BasketId { get; set; }
}