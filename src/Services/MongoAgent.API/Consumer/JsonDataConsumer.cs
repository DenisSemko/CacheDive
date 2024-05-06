namespace MongoAgent.API.Consumer;

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

                    if (await _unitOfWork.Users.GetByIdAsync(user.Id) is null)
                    {
                        await _unitOfWork.Users.InsertOneAsync(user);
                    }
                    break;
                case nameof(Category):
                    DeserializeParsedDataHelper<Category> categoryHelper = new();
                    Category category = categoryHelper.DeserializeData(tuple.Item2.ToString());

                    if (await _unitOfWork.Categories.GetByIdAsync(category.Id) is null)
                    {
                        await _unitOfWork.Categories.InsertOneAsync(category);
                    }
                    break;
                case nameof(Product):
                    DeserializeParsedDataHelper<Product> productHelper = new();
                    Product product = productHelper.DeserializeData(tuple.Item2.ToString());
                    
                    if (await _unitOfWork.Products.GetByIdAsync(product.Id) is null)
                    {
                        await _unitOfWork.Products.InsertOneAsync(product);
                    }
                    break;
                case nameof(Basket):
                    DeserializeParsedDataHelper<Basket> basketHelper = new();
                    Basket basket = basketHelper.DeserializeData(tuple.Item2.ToString());
                    
                    if (await _unitOfWork.Baskets.GetByIdAsync(basket.Id) is null)
                    {
                        await _unitOfWork.Baskets.InsertOneAsync(basket);
                    }
                    break;
                case nameof(ProductBasket):
                    DeserializeParsedDataHelper<ProductBasket> productBasketHelper = new();
                    ProductBasket productBasket = productBasketHelper.DeserializeData(tuple.Item2.ToString());
                    
                    if (await _unitOfWork.ProductBaskets.GetByIdAsync(productBasket.Id) is null)
                    {
                        await _unitOfWork.ProductBaskets.InsertOneAsync(productBasket);
                    }
                    break;
                case nameof(Order):
                    DeserializeParsedDataHelper<Order> orderHelper = new();
                    Order order = orderHelper.DeserializeData(tuple.Item2.ToString());
                    
                    if (await _unitOfWork.Orders.GetByIdAsync(order.Id) is null)
                    {
                        await _unitOfWork.Orders.InsertOneAsync(order);
                    }
                    break;
                case nameof(ProductOrder):
                    DeserializeParsedDataHelper<ProductOrder> productOrderHelper = new();
                    ProductOrder productOrder = productOrderHelper.DeserializeData(tuple.Item2.ToString());
                    
                    if (await _unitOfWork.ProductOrders.GetByIdAsync(productOrder.Id) is null)
                    {
                        await _unitOfWork.ProductOrders.InsertOneAsync(productOrder);
                    }
                    break;
            }
        }
        parsedData.Clear();
    }
}