using BallastLaneApplication.Domain.DTOs;
using BallastLaneApplication.Domain.Entities;

namespace BallastLaneApplication.Data.Service.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllAsync(string email);
        Task<Product> GetAsync(string id, string email);
        Task AddAsync(Product product, string email);
        Task<bool> UpdateAsync(ProductDTO dto, string email);
        Task<bool> DeleteAsync(string id, string email);
    }
}
