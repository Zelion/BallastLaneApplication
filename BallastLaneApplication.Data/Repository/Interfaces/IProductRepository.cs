using BallastLaneApplication.Domain.Entities;

namespace BallastLaneApplication.Data.Repository.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetProductsAsync();
        Task<Product> GetProductByIdAsync(string id);
    }
}
