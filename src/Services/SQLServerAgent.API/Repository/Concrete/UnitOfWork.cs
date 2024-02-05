namespace SQLServerAgent.API.Repository.Concrete;

public class UnitOfWork : IUnitOfWork
{
    #region Private fields
    private readonly ApplicationContext _applicationContext;
    #endregion

    #region ctor
    public UnitOfWork(ApplicationContext applicationContext)
    {
        _applicationContext = applicationContext;
    }
    #endregion

    #region Repositories
    
    private IBaseRepository<User> _userRepository;
    public IBaseRepository<User> Users => _userRepository ?? new BaseRepository<User>(_applicationContext);
    
    private IBaseRepository<Category> _categoryRepository;
    public IBaseRepository<Category> Categories => _categoryRepository ?? new BaseRepository<Category>(_applicationContext);
    
    private IBaseRepository<Product> _productRepository;
    public IBaseRepository<Product> Products => _productRepository ?? new BaseRepository<Product>(_applicationContext);
    
    private IBaseRepository<Basket> _basketRepository;
    public IBaseRepository<Basket> Baskets => _basketRepository ?? new BaseRepository<Basket>(_applicationContext);
    
    private IBaseRepository<ProductBasket> _productBasketRepository;
    public IBaseRepository<ProductBasket> ProductBaskets => _productBasketRepository ?? new BaseRepository<ProductBasket>(_applicationContext);
    
    private IBaseRepository<Order> _orderRepository;
    public IBaseRepository<Order> Orders => _orderRepository ?? new BaseRepository<Order>(_applicationContext);
    
    private IBaseRepository<ProductOrder> _productOrderRepository;
    public IBaseRepository<ProductOrder> ProductOrders => _productOrderRepository ?? new BaseRepository<ProductOrder>(_applicationContext);
    
    #endregion
}