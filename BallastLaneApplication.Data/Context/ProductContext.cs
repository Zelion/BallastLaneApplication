using BallastLaneApplication.Domain.Entities;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace BallastLaneApplication.Data.Context
{
    public class ProductContext : IProductContext
    {
        public ProductContext(IConfiguration configuration)
        {
            var connectionString = configuration.GetSection("DabaseSettings:ConnectionString");
            var databaseName = configuration.GetSection("DabaseSettings:DatabaseName");

            var client = new MongoClient(connectionString.Value);
            var database = client.GetDatabase(databaseName.Value);

            Products = database.GetCollection<Product>("Products");
            Users = database.GetCollection<User>("Users");
        }

        public IMongoCollection<Product> Products { get; }
        public IMongoCollection<User> Users { get; }
    }
}