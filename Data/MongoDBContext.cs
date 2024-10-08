using MongoDB.Driver;
using HealthyBites._Models;

public class MongoDBContext
{
    private readonly IMongoDatabase _database;

    public MongoDBContext(IConfiguration configuration)
    {
        var client = new MongoClient(configuration.GetSection("MongoDB:ConnectionString").Value);
        _database = client.GetDatabase(configuration.GetSection("MongoDB:DatabaseName").Value);
    }

    public IMongoCollection<User> Users => _database.GetCollection<User>("Users");
    public IMongoCollection<Order> Orders => _database.GetCollection<Order>("Orders");
    public IMongoCollection<Product> Products => _database.GetCollection<Product>("Products");
    public IMongoCollection<ProductCat> ProductCats => _database.GetCollection<ProductCat>("ProductCatesgories");
    public IMongoCollection<Feedback> Feedback => _database.GetCollection<Feedback>("Feedback");
    public IMongoCollection<Notification> Notifications => _database.GetCollection<Notification>("Notifications");

}
