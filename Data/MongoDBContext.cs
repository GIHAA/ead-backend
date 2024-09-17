using MongoDB.Driver;

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

}
