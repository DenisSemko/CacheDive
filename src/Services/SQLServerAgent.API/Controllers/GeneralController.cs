namespace SQLServerAgent.API.Controllers;

/// <summary>
/// Controller for General operations.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class GeneralController : Controller
{
    #region PrivateFields
    
    private readonly IUnitOfWork _unitOfWork;
    
    #endregion
    
    #region ctor
    
    public GeneralController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }
    
    #endregion
    
    #region ControllerMethods
    
    [HttpGet]
    public async Task<ActionResult<Dictionary<Type, List<object>>>> GetAllAsync()
    {
        Dictionary<Type, List<object>> result = new();
        IReadOnlyList<User> users = await _unitOfWork.Users.GetAllAsync();
        IReadOnlyList<Category> categories = await _unitOfWork.Categories.GetAllAsync();
        IReadOnlyList<Product> products = await _unitOfWork.Products.GetAllAsync();
        IReadOnlyList<Basket> baskets = await _unitOfWork.Baskets.GetAllAsync();
        IReadOnlyList<ProductBasket> productBaskets = await _unitOfWork.ProductBaskets.GetAllAsync();
        IReadOnlyList<Order> orders = await _unitOfWork.Orders.GetAllAsync();
        IReadOnlyList<ProductOrder> productOrders = await _unitOfWork.ProductOrders.GetAllAsync();

        if (users.Any() && categories.Any() && products.Any() && baskets.Any() && productBaskets.Any() && orders.Any() && productOrders.Any())
        {
            result.Add(typeof(User), users.Cast<object>().ToList());
            result.Add(typeof(Category), categories.Cast<object>().ToList());
            result.Add(typeof(Product), products.Cast<object>().ToList());
            result.Add(typeof(Basket), baskets.Cast<object>().ToList());
            result.Add(typeof(ProductBasket), productBaskets.Cast<object>().ToList());
            result.Add(typeof(Order), orders.Cast<object>().ToList());
            result.Add(typeof(ProductOrder), productOrders.Cast<object>().ToList());
        }

        return result;
    }
    
    #endregion
}