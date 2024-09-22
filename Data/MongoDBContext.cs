using MongoDB.Driver;
using TechFixBackend._Models;

public class MongoDBContext
{
    private readonly IMongoDatabase _database;

    public MongoDBContext(IConfiguration configuration)
    {
        var client = new MongoClient(configuration.GetSection("MongoDB:ConnectionString").Value);
        _database = client.GetDatabase(configuration.GetSection("MongoDB:DatabaseName").Value);
    }

    public IMongoCollection<User> Users => _database.GetCollection<User>("Users");
    public IMongoCollection<Vendor> Vendors => _database.GetCollection<Vendor>("Vendors");
    public IMongoCollection<Product> Products => _database.GetCollection<Product>("Products");


}
