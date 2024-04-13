namespace RedisAgent.API.Consumer;

public class JsonDataConsumer : IConsumer<JsonDataPublishedEvent>
{
    private readonly IJsonToTupleService _jsonToTupleService;
    private readonly IUnitOfWork _unitOfWork;

    public JsonDataConsumer(IJsonToTupleService jsonToTupleService, IUnitOfWork unitOfWork)
    {
        _jsonToTupleService = jsonToTupleService ?? throw new ArgumentNullException(nameof(jsonToTupleService));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }
    
    public async Task Consume(ConsumeContext<JsonDataPublishedEvent> context)
    {
        JsonDataPublishedEvent data = context.Message;
        List<Tuple<string, JObject>> parsedData = _jsonToTupleService.ParseJson(data.DatabaseData);

        foreach (var tuple in parsedData)
        {
            switch (tuple.Item1)
            {
                case nameof(User):
                    DeserializeParsedDataHelper<User> userHelper = new();
                    User user = userHelper.DeserializeData(tuple.Item2.ToString());

                    if (await _unitOfWork.Users.GetByKeyAsync($"{nameof(User)}:{user.Id}") is null)
                    {
                        await _unitOfWork.Users.SetAsync($"{nameof(User)}:{user.Id}", user);
                    }
                    break;
                case nameof(Category):
                    DeserializeParsedDataHelper<Category> categoryHelper = new();
                    Category category = categoryHelper.DeserializeData(tuple.Item2.ToString());

                    if (await _unitOfWork.Categories.GetByKeyAsync($"{nameof(Category)}:{category.Id}") is null)
                    {
                        await _unitOfWork.Categories.SetAsync($"{nameof(Category)}:{category.Id}", category);
                    }
                    break;
                case nameof(Product):
                    DeserializeParsedDataHelper<Product> productHelper = new();
                    Product product = productHelper.DeserializeData(tuple.Item2.ToString());
                    
                    if (await _unitOfWork.Products.GetByKeyAsync($"{nameof(Product)}:{product.Id}") is null)
                    {
                        await _unitOfWork.Products.SetAsync($"{nameof(Product)}:{product.Id}", product);
                    }
                    break;
                case nameof(Basket):
                    DeserializeParsedDataHelper<Basket> basketHelper = new();
                    Basket basket = basketHelper.DeserializeData(tuple.Item2.ToString());
                    
                    if (await _unitOfWork.Baskets.GetByKeyAsync($"{nameof(Basket)}:{basket.Id}") is null)
                    {
                        await _unitOfWork.Baskets.SetAsync($"{nameof(Basket)}:{basket.Id}", basket);
                    }
                    break;
                case nameof(ProductBasket):
                    DeserializeParsedDataHelper<ProductBasket> productBasketHelper = new();
                    ProductBasket productBasket = productBasketHelper.DeserializeData(tuple.Item2.ToString());
                    
                    if (await _unitOfWork.ProductBaskets.GetByKeyAsync($"{nameof(ProductBasket)}:{productBasket.Id}") is null)
                    {
                        await _unitOfWork.ProductBaskets.SetAsync($"{nameof(ProductBasket)}:{productBasket.Id}", productBasket);
                    }
                    break;
                case nameof(Order):
                    DeserializeParsedDataHelper<Order> orderHelper = new();
                    Order order = orderHelper.DeserializeData(tuple.Item2.ToString());
                    
                    if (await _unitOfWork.Orders.GetByKeyAsync($"{nameof(Order)}:{order.Id}") is null)
                    {
                        await _unitOfWork.Orders.SetAsync($"{nameof(Order)}:{order.Id}", order);
                    }
                    break;
                case nameof(ProductOrder):
                    DeserializeParsedDataHelper<ProductOrder> productOrderHelper = new();
                    ProductOrder productOrder = productOrderHelper.DeserializeData(tuple.Item2.ToString());
                    
                    if (await _unitOfWork.ProductOrders.GetByKeyAsync($"{nameof(ProductOrder)}:{productOrder.Id}") is null)
                    {
                        await _unitOfWork.ProductOrders.SetAsync($"{nameof(ProductOrder)}:{productOrder.Id}", productOrder);
                    }
                    break;
            }
        }
        parsedData.Clear();
    }

   
}