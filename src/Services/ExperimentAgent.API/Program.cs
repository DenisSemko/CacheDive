var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAgentService(builder.Configuration);
builder.Services.AddMassTransit(configuration => {
    configuration.AddRequestClient<GetSqlServerExecutionRequest>();
    configuration.AddRequestClient<GetRedisExecutionRequest>();
    configuration.AddRequestClient<GetMongoDbExecutionRequest>();
    
    configuration.UsingRabbitMq((context, configurator) => {
        configurator.Host(builder.Configuration["EventBusSettings:HostAddress"]);
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