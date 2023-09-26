using BallastLaneApplication.Data.Repository.Interfaces;
using BallastLaneApplication.Data.Service.Interfaces;
using BallastLaneApplication.Domain.DTOs;
using BallastLaneApplication.Domain.Entities;

namespace BallastLaneApplication.Data.Service
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;

        public ProductService(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Product>> GetProductsAsync(string userId)
        {
            return await _repository.GetProductsAsync(userId);
        }

        public async Task<Product> GetProductAsync(string id, string userId)
        {
            return await _repository.GetProductAsync(id, userId);
        }

        public async Task AddProduct(Product product)
        {
            await _repository.AddProductAsync(product);
        }

        public async Task<bool> UpdateProductAsync(ProductDTO dto, string userId)
        {
            var product = await _repository.GetProductAsync(dto.Id, userId);
            if (product == null)
            {
                return false;
            }

            dto.Update(product, userId);

            return await _repository.UpdateProductAsync(product);
        }

        public async Task<bool> DeleteProductAsync(string id, string userId)
        {
            return await _repository.DeleteProductAsync(id, userId);
        }
    }
}
