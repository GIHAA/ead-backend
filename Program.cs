using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using TechFixBackend.Hubs;
using TechFixBackend.Repository;
using TechFixBackend.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "TechFixBackend", Version = "v1" });
});

// Add SignalR
builder.Services.AddSignalR();

// MongoDB context
builder.Services.AddSingleton<MongoDBContext>();

// Register the IUserRepository and its implementation
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Register the NotificationService
builder.Services.AddScoped<NotificationService>();

// Register AuthService with proper DI
var key = builder.Configuration["JwtKey"];
builder.Services.AddScoped<AuthService>(provider =>
    new AuthService(
        provider.GetRequiredService<IUserRepository>(),
        provider.GetRequiredService<NotificationService>(),
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
});

// Add Controllers
builder.Services.AddControllers();

// Add Vendor Repository and Service
builder.Services.AddScoped<IVendorRepository, VendorRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IFeedbackRepository, FeedbackRepository>();

// Add Service
builder.Services.AddScoped<IVendorService, VendorService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<FeedbackService>();

// Add CORS policy to allow requests from the Android emulator
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowEmulator",
        builder =>
        {
            builder.WithOrigins("https://10.0.2.2:5215", "http://10.0.2.2:5215") // Allow both HTTPS and HTTP requests from Android emulator
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowEmulator");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// Configure SignalR endpoints
app.MapHub<NotificationHub>("/notifications");

app.UseHttpsRedirection();
app.Run();
