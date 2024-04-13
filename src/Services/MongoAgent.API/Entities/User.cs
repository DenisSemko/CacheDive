namespace MongoAgent.API.Entities;

[BsonCollection("User")]
public class User
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public string UserLogin { get; set; }
    public string PasswordHash { get; set; }
    public DateTime RegistrationDate { get; set; } = DateTime.Now;
}