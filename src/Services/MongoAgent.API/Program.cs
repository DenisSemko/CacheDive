var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAgentServices(builder.Configuration);
builder.Services.AddMassTransit(configuration => {
    configuration.AddConsumer<JsonDataConsumer>();
    configuration.AddConsumer<GetMongoDbExecutionConsumer>();
    
    configuration.UsingRabbitMq((context, configurator) => {
        configurator.Host(builder.Configuration["EventBusSettings:HostAddress"]);
        
        configurator.ReceiveEndpoint(EventBus.Messages.Common.Constants.TransferMongoJsonQueue, c => {
            c.ConfigureConsumer<JsonDataConsumer>(context);
        });
        
        configurator.ReceiveEndpoint(EventBus.Messages.Common.Constants.GetMongoDbExecutionQueue, c => {
            c.ConfigureConsumer<GetMongoDbExecutionConsumer>(context);
        });
    });
});

builder.Services.AddControllers().AddJsonOptions(x =>
{
    x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();