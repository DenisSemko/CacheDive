namespace MongoAgent.API.Entities;

[BsonCollection("Product")]
public class Product
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public double Price { get; set; }
    public string Image { get; set; }
    public int Quantity { get; set; }
    [BsonRepresentation(BsonType.String)]
    public Guid CategoryId { get; set; }
}