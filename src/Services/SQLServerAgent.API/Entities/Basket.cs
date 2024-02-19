namespace SQLServerAgent.API.Entities;

public class Basket
{
    public Guid Id { get; set; }
    public int TotalQuantity { get; set; }
    public double TotalPrice { get; set; }
    public Guid UserId { get; set; }
}