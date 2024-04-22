namespace IdentityServer.API.Common;

public static class AgentServicesRegistration
{
    public static IServiceCollection AddAgentServices(this IServiceCollection services,
        ConfigurationManager configuration)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        
        services.AddDbContext<ApplicationContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        
        services.AddIdentity<ApplicationUser, IdentityRole<Guid>>()
            .AddEntityFrameworkStores<ApplicationContext>()
            .AddDefaultTokenProviders();
        
        services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        
        services.Configure<JwtOptions>(configuration.GetSection("JWT"));
        services.AddSingleton<IJwtOptions>(serviceProvider =>
            serviceProvider.GetRequiredService<IOptions<JwtOptions>>().Value);

        services.AddScoped<ITokenGeneratorService, TokenGeneratorService>();
        services.AddScoped<IAccessTokenService, AccessTokenService>();
        services.AddScoped<IRefreshTokenService, RefreshTokenService>();
        services.AddScoped<IAuthenticationResultService, AuthenticationResultService>();
        services.AddScoped<IAuthService, AuthService>();
        
        services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }
        ).AddJwtBearer(x =>
        {
            x.RequireHttpsMetadata = false;
            x.SaveToken = true;
            x.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"])),
                ValidateIssuer = true,
                ValidIssuer = configuration["JWT:ValidIssuer"],
                ValidateAudience = true,
                ValidAudience = configuration["JWT:ValidAudience"],
                ClockSkew = TimeSpan.Zero
            };
        });
        
        return services;
    }
}