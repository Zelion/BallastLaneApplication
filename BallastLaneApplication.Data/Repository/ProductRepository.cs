using BallastLaneApplication.Data.Context;
using BallastLaneApplication.Data.Repository.Interfaces;
using BallastLaneApplication.Domain.Entities;
using MongoDB.Driver;

namespace BallastLaneApplication.Data.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly IProductContext _productContext;

        public ProductRepository(IProductContext productContext)
        {
            _productContext = productContext ?? throw new ArgumentNullException(nameof(productContext));
        }

        public async Task<IEnumerable<Product>> GetProductsAsync(string userId)
        {
            return await _productContext.Products.Find(x => x.UserId.Equals(userId)).ToListAsync();
        }

        public async Task<Product> GetProductAsync(string id, string userId)
        {
            return await _productContext.Products.Find(x => x.Id.Equals(id) && x.UserId.Equals(userId)).FirstOrDefaultAsync();
        }

        public async Task AddProductAsync(Product product)
        {
            await _productContext.Products.InsertOneAsync(product);
        }

        public async Task<bool> UpdateProductAsync(Product product)
        {
            var replaceAsync = await _productContext.Products.ReplaceOneAsync(filter: x => x.Id.Equals(product.Id), product);

            return replaceAsync.IsAcknowledged && replaceAsync.ModifiedCount > 0;
        }

        public async Task<bool> DeleteProductAsync(string id, string userId)
        {
            var filter = Builders<Product>.Filter.Where(x => x.Id.Equals(id) && x.UserId.Equals(userId));

            var deleteResult = await _productContext.Products.DeleteOneAsync(filter);

            return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
        }
    }
}
