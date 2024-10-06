using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
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

// Add CORS services
builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactJSDomain", policy =>
    {
        policy.WithOrigins("http://localhost:5215", "http://localhost:5173")
                  .AllowAnyMethod()
                  .AllowAnyHeader()
                  .AllowCredentials();
    });
});


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

    // Extract the token from the SignalR query string
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

// Add  Repository and Service
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IVendorRepository, VendorRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IFeedbackRepository, FeedbackRepository>();
builder.Services.AddScoped<IProductCatRepository, ProductCatRepository>();

// Add Service
builder.Services.AddScoped<IVendorService, VendorService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<FeedbackService>();
builder.Services.AddScoped<IProductCatService, ProductCatService>();
builder.Services.AddSingleton<IUserIdProvider, CustomUserIdProvider>();

// Register NotificationManager as a singleton
builder.Services.AddSingleton<NotificationManager>();

// Register NotificationService
builder.Services.AddScoped<NotificationService>();

// Add CORS policy to allow requests from the Android emulator
/*builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowEmulator",
        builder =>
        {
            builder.WithOrigins("https://10.0.2.2:5215", "http://10.0.2.2:5215")
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });

    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});*/

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
//app.UseCors("AllowSwaggerUI");
app.UseCors("ReactJSDomain");
//app.MapControllers().RequireCors("AllowEmulator");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Configure SignalR endpoints
app.MapHub<NotificationHub>("/notifications").RequireCors("ReactJSDomain"); ;

app.UseHttpsRedirection();
app.Run();
