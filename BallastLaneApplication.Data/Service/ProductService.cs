using BallastLaneApplication.Data.Repository.Interfaces;
using BallastLaneApplication.Data.Service.Interfaces;
using BallastLaneApplication.Domain.DTOs;
using BallastLaneApplication.Domain.Entities;

namespace BallastLaneApplication.Data.Service
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;
        private readonly IUserService _userService;

        public ProductService(
            IProductRepository repository,
            IUserService userService
            )
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(_repository));
            _userService = userService ?? throw new ArgumentNullException(nameof(_userService));
        }

        public async Task<IEnumerable<Product>> GetAllAsync(string email)
        {
            var user = await _userService.GetByEmailAsync(email);
            if (user == null)
                throw new KeyNotFoundException("User not found");

            return await _repository.GetProductsAsync(user.Id);
        }

        public async Task<Product> GetAsync(string id, string email)
        {
            var user = await _userService.GetByEmailAsync(email);
            if (user == null)
                throw new KeyNotFoundException("User not found");

            try
            {
                return await _repository.GetProductAsync(id, user.Id);
            }
            catch (FormatException)
            {
                return null;
            }
        }

        public async Task AddAsync(Product product, string email)
        {
            var user = await _userService.GetByEmailAsync(email);
            if (user == null)
                throw new KeyNotFoundException("User not found");

            SetDefaultValues(product, user.Id);
            await _repository.AddProductAsync(product);
        }

        public async Task<bool> UpdateAsync(ProductDTO dto, string email)
        {
            var user = await _userService.GetByEmailAsync(email);
            if (user == null)
                throw new KeyNotFoundException("User not found");

            var product = await _repository.GetProductAsync(dto.Id, user.Id);
            if (product == null)
            {
                return false;
            }

            dto.Update(product, user.Id);

            return await _repository.UpdateProductAsync(product);
        }

        public async Task<bool> DeleteAsync(string id, string email)
        {
            var user = await _userService.GetByEmailAsync(email);
            if (user == null)
                throw new KeyNotFoundException("User not found");

            return await _repository.DeleteProductAsync(id, user.Id);
        }

        #region Private Methods

        private void SetDefaultValues(Product product, string userId)
        {
            product.UserId = userId;
            product.Created = DateTime.Now;
            product.CreatedBy = userId;
            product.Modified = DateTime.Now;
            product.ModifiedBy = userId;
        }

        #endregion
    }
}
