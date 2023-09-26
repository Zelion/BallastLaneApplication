using BallastLaneApplication.Domain.Entities;
using MongoDB.Driver;

namespace BallastLaneApplication.Data.Context
{
    public interface IProductContext
    {
        IMongoCollection<Product> Products { get; }
        IMongoCollection<User> Users { get; }
    }
}
