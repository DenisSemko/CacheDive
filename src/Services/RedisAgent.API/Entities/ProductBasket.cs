namespace RedisAgent.API.Entities;

public class ProductBasket
{
    public Guid Id { get; set; }
    public int Quantity { get; set; }
    public double Price { get; set; }
    public Guid ProductId { get; set; }
    public Guid BasketId { get; set; }
}