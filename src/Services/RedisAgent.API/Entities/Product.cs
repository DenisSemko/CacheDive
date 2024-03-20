namespace RedisAgent.API.Entities;

public class Product
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public double Price { get; set; }
    public string Image { get; set; }
    public int Quantity { get; set; }
    public Guid CategoryId { get; set; }
}