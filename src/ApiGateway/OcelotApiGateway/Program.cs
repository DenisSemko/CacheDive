var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "../../SharedConfig", "Config.json"), true, true);
builder.Configuration.AddJsonFile($"ocelot.json", true, true);
builder.Services.AddOcelot().AddCacheManager(settings => settings.WithDictionaryHandle());
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

var app = builder.Build();

app.UseCors("AllowAll");

app.AddCustomMiddleware();

app.UseOcelot();

app.Run();