using AutoFixture;
using BallastLaneApplication.Data.Repository.Interfaces;
using BallastLaneApplication.Data.Service;
using BallastLaneApplication.Data.Service.Interfaces;
using BallastLaneApplication.Domain.Entities;
using Moq;

namespace BallastLaneApplication.UnitTesting
{
    public class ProductServiceTest
    {
        private Fixture _fixture;
        private readonly IProductService _productService;
        private Mock<IProductRepository> _mockProductRepository;
        private Mock<IUserService> _mockUserService;

        public ProductServiceTest()
        {
            // fixture for creating test data
            _fixture = new Fixture();

            // mock dependencies
            _mockProductRepository = new Mock<IProductRepository>();
            _mockUserService = new Mock<IUserService>();

            // service under test
            _productService = new ProductService(_mockProductRepository.Object, _mockUserService.Object);
        }

        [Theory]
        [InlineData("testUserId", "test@email.com")]
        public async Task GetProductsAsync_ReturnsAllProducts(string userId, string email)
        {
            // Arrange
            var userFixture = _fixture.Build<User>().With(x => x.Email, email)
                                                    .With(x => x.Id, userId)
                                                    .Create();
            var productsFixture = _fixture.Build<Product>().With(x => x.UserId, userId)
                                                           .CreateMany(3);

            _mockProductRepository.Setup(x => x.GetProductsAsync(userId)).ReturnsAsync(productsFixture);
            _mockUserService.Setup(x => x.GetByEmailAsync(email)).ReturnsAsync(userFixture);

            // Act
            var products = await _productService.GetAllAsync(userFixture.Email);

            // Assert
            Assert.True(products.Count() == 3);
            Assert.Equal(productsFixture.First().Name, products.First().Name);
        }

        [Theory]
        [InlineData("testUserId", "test@email.com")]
        public async Task GetProductAsync_ValidId_ReturnsProduct(string userId, string email)
        {
            // Arrange
            var userFixture = _fixture.Build<User>().With(x => x.Email, email)
                                                    .With(x => x.Id, userId)
                                                    .Create();

            var productFixture = _fixture.Create<Product>();
            var id = productFixture.Id;

            _mockProductRepository.Setup(x => x.GetProductAsync(id, userId)).ReturnsAsync(productFixture);
            _mockUserService.Setup(x => x.GetByEmailAsync(email)).ReturnsAsync(userFixture);

            // Act
            var product = await _productService.GetAsync(id, email);

            // Assert
            Assert.NotNull(product);
            Assert.Equal(productFixture.Name, product.Name);
        }

        [Fact]
        public async Task GetProductAsync_InvalidId_ThrowsException()
        {
            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _productService.GetAsync("asdasdasasd", "test@email.com"));
        }
    }
}