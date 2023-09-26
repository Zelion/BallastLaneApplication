using BallastLaneApplication.Domain.DTOs;
using BallastLaneApplication.Domain.Entities;

namespace BallastLaneApplication.Data.Service.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetProductsAsync(string userId);
        Task<Product> GetProductAsync(string id, string userId);
        Task AddProduct(Product product);
        Task<bool> UpdateProductAsync(ProductDTO dto, string userId);
        Task<bool> DeleteProductAsync(string id, string userId);
    }
}
