var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAgentServices(builder.Configuration);

builder.Services.AddMassTransit(configuration => {
    configuration.UsingRabbitMq((context, configurator) => {
        configurator.Host(builder.Configuration["EventBusSettings:HostAddress"]);
    });
});

builder.Services.AddCors(
    options =>
    {
        options.AddPolicy(name: "AllowAll", builder =>
        {
            builder.WithOrigins("https://localhost:3000")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
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

app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();