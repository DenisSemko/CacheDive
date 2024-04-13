namespace MongoAgent.API.Entities;

[BsonCollection("Category")]
public class Category
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; set; }
    public string Name { get; set; }
}