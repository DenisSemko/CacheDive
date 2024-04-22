namespace MongoAgent.API.Entities;

[BsonCollection("Basket")]
public class Basket
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; set; }
    public int TotalQuantity { get; set; }
    public double TotalPrice { get; set; }
    [BsonRepresentation(BsonType.String)]
    public Guid UserId { get; set; }
}