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

        public async Task<Product> GetProductByIdAsync(string id)
        {
            return await _productContext.Products.Find(p => p.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            return await _productContext.Products.Find(x => true).ToListAsync();
        }
    }
}
