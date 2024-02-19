namespace SQLServerAgent.API.Entities;

public class ProductOrder
{
    public Guid Id { get; set; }
    public int Quantity { get; set; }
    public double Price { get; set; }
    public Guid ProductId { get; set; }
    public Guid OrderId { get; set; }
}