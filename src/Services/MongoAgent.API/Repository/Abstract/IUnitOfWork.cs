namespace MongoAgent.API.Repository.Abstract;

public interface IUnitOfWork
{
    IBaseRepository<User> Users { get; }
    IBaseRepository<Category> Categories { get; }
    IBaseRepository<Product> Products { get; }
    IBaseRepository<Basket> Baskets { get; }
    IBaseRepository<ProductBasket> ProductBaskets { get; }
    IBaseRepository<Order> Orders { get; }
    IBaseRepository<ProductOrder> ProductOrders { get; }
}