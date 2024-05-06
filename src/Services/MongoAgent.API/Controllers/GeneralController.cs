namespace MongoAgent.API.Controllers;

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
    public async Task<ActionResult<Dictionary<string, List<object>>>> GetAllAsync()
    {
        Dictionary<string, List<object>> result = new();
        IReadOnlyList<User> users = await _unitOfWork.Users.GetAllAsync();
        IReadOnlyList<Category> categories = await _unitOfWork.Categories.GetAllAsync();
        IReadOnlyList<Product> products = await _unitOfWork.Products.GetAllAsync();
        IReadOnlyList<Basket> baskets = await _unitOfWork.Baskets.GetAllAsync();
        IReadOnlyList<ProductBasket> productBaskets = await _unitOfWork.ProductBaskets.GetAllAsync();
        IReadOnlyList<Order> orders = await _unitOfWork.Orders.GetAllAsync();
        IReadOnlyList<ProductOrder> productOrders = await _unitOfWork.ProductOrders.GetAllAsync();

        if (users.Any() && categories.Any() && products.Any() && baskets.Any() && productBaskets.Any() && orders.Any() && productOrders.Any())
        {
            result.Add(nameof(User), users.Cast<object>().ToList());
            result.Add(nameof(Category), categories.Cast<object>().ToList());
            result.Add(nameof(Product), products.Cast<object>().ToList());
            result.Add(nameof(Basket), baskets.Cast<object>().ToList());
            result.Add(nameof(ProductBasket), productBaskets.Cast<object>().ToList());
            result.Add(nameof(Order), orders.Cast<object>().ToList());
            result.Add(nameof(ProductOrder), productOrders.Cast<object>().ToList());
        }

        return result;
    }
    
    #endregion
}