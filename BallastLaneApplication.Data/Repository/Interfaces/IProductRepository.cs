using BallastLaneApplication.Domain.Entities;

namespace BallastLaneApplication.Data.Repository.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetProductsAsync(string userId);
        Task<Product> GetProductAsync(string id, string userId);
        Task AddProductAsync(Product product);
        Task<bool> UpdateProductAsync(Product product);
        Task<bool> DeleteProductAsync(string id, string userId);
    }
}
