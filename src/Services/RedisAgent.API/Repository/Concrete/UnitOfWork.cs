using StackExchange.Redis;
using Order = RedisAgent.API.Entities.Order;

namespace RedisAgent.API.Repository.Concrete;

public class UnitOfWork : IUnitOfWork
{
    #region Private fields
    private readonly IConnectionMultiplexer _connectionMultiplexer;
    #endregion

    #region ctor
    public UnitOfWork(IConnectionMultiplexer connectionMultiplexer)
    {
        _connectionMultiplexer = connectionMultiplexer;
    }
    #endregion

    #region Repositories
    
    private IBaseRepository<User> _userRepository;
    public IBaseRepository<User> Users => _userRepository ?? new BaseRepository<User>(_connectionMultiplexer);
    
    private IBaseRepository<Category> _categoryRepository;
    public IBaseRepository<Category> Categories => _categoryRepository ?? new BaseRepository<Category>(_connectionMultiplexer);
    
    private IBaseRepository<Product> _productRepository;
    public IBaseRepository<Product> Products => _productRepository ?? new BaseRepository<Product>(_connectionMultiplexer);
    
    private IBaseRepository<Basket> _basketRepository;
    public IBaseRepository<Basket> Baskets => _basketRepository ?? new BaseRepository<Basket>(_connectionMultiplexer);
    
    private IBaseRepository<ProductBasket> _productBasketRepository;
    public IBaseRepository<ProductBasket> ProductBaskets => _productBasketRepository ?? new BaseRepository<ProductBasket>(_connectionMultiplexer);
    
    private IBaseRepository<Order> _orderRepository;
    public IBaseRepository<Order> Orders => _orderRepository ?? new BaseRepository<Order>(_connectionMultiplexer);
    
    private IBaseRepository<ProductOrder> _productOrderRepository;
    public IBaseRepository<ProductOrder> ProductOrders => _productOrderRepository ?? new BaseRepository<ProductOrder>(_connectionMultiplexer);
    
    #endregion
}