namespace MongoAgent.API.Repository.Concrete;

public class UnitOfWork : IUnitOfWork
{
    #region Private fields
    private readonly IDbSettings _settings;
    #endregion

    #region ctor
    public UnitOfWork(IDbSettings settings)
    {
        _settings = settings;
    }
    #endregion

    #region Repositories
    
    private IBaseRepository<User> _userRepository;
    public IBaseRepository<User> Users => _userRepository ?? new BaseRepository<User>(_settings);
    
    private IBaseRepository<Category> _categoryRepository;
    public IBaseRepository<Category> Categories => _categoryRepository ?? new BaseRepository<Category>(_settings);
    
    private IBaseRepository<Product> _productRepository;
    public IBaseRepository<Product> Products => _productRepository ?? new BaseRepository<Product>(_settings);
    
    private IBaseRepository<Basket> _basketRepository;
    public IBaseRepository<Basket> Baskets => _basketRepository ?? new BaseRepository<Basket>(_settings);
    
    private IBaseRepository<ProductBasket> _productBasketRepository;
    public IBaseRepository<ProductBasket> ProductBaskets => _productBasketRepository ?? new BaseRepository<ProductBasket>(_settings);
    
    private IBaseRepository<Order> _orderRepository;
    public IBaseRepository<Order> Orders => _orderRepository ?? new BaseRepository<Order>(_settings);
    
    private IBaseRepository<ProductOrder> _productOrderRepository;
    public IBaseRepository<ProductOrder> ProductOrders => _productOrderRepository ?? new BaseRepository<ProductOrder>(_settings);
    
    #endregion
}