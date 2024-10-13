using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using HealthyBites.Hubs;
using HealthyBites.Repository;
using HealthyBites.Services;
using Microsoft.Extensions.Diagnostics.HealthChecks;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "HealthyBites", Version = "v1" });
});

// Add SignalR
builder.Services.AddSignalR();

// MongoDB context
builder.Services.AddSingleton<MongoDBContext>();

// Register the NotificationService
builder.Services.AddScoped<NotificationService>();

// Register AuthService with proper DI
var key = builder.Configuration["JwtKey"];
builder.Services.AddScoped<AuthService>(provider =>
    new AuthService(
        provider.GetRequiredService<IUserRepository>(),
        provider.GetRequiredService<NotificationService>(),
        provider.GetRequiredService<IProductRepository>(),
        key
    ));

// Configure JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
        ValidateIssuer = false,
        ValidateAudience = false
    };

    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Query["access_token"];
            var path = context.HttpContext.Request.Path;
            if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/notifications"))
            {
                context.Token = accessToken;
            }
            return Task.CompletedTask;
        }
    };
});

// Add Controllers
builder.Services.AddControllers();

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactJSDomain", policy =>
    {
        policy.WithOrigins("http://localhost:5215", "http://localhost:5173")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });

    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Register repositories and services
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IVendorRepository, VendorRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IFeedbackRepository, FeedbackRepository>();
builder.Services.AddScoped<IProductCatRepository, ProductCatRepository>();

builder.Services.AddScoped<IVendorService, VendorService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<FeedbackService>();
builder.Services.AddScoped<IProductCatService, ProductCatService>();
builder.Services.AddSingleton<IUserIdProvider, CustomUserIdProvider>();

// Add health checks
builder.Services.AddHealthChecks()
    .AddCheck("MongoDB", () =>
    {
        // Example check for MongoDB
        return HealthCheckResult.Healthy("MongoDB is OK");
    });

// Build the app
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("ReactJSDomain");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<NotificationHub>("/notifications").RequireCors("ReactJSDomain");

// Map health check endpoint
app.MapHealthChecks("/health");

app.Run();
