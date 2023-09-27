using AutoFixture;
using AutoMapper;
using BallastLaneApplication.Controllers;
using BallastLaneApplication.Data.Service.Interfaces;
using BallastLaneApplication.Domain.DTOs;
using BallastLaneApplication.Domain.Entities;
using BallastLaneApplication.Mapping;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Security.Claims;

namespace BallastLaneApplication.UnitTesting.ProductTest
{
    public class ProductControllerTest
    {
        private Fixture _fixture;
        private readonly ProductController _productController;
        private Mock<IProductService> _mockProductService;
        private Mock<IUserService> _mockUserService;
        private Mock<ILogger<ProductController>> _logger;

        public ProductControllerTest()
        {
            // fixture for creating test data
            _fixture = new Fixture();

            // automapper dependency
            var mapper = new MapperConfiguration(x => x.AddProfile<MapperConfig>()).CreateMapper();

            // mock dependencies
            _mockProductService = new Mock<IProductService>();
            _mockUserService = new Mock<IUserService>();
            _logger = new Mock<ILogger<ProductController>>();

            // claims
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] {
                                        new Claim(ClaimTypes.NameIdentifier, "test@email.com"),
                                        new Claim(ClaimTypes.Name, "test@email.com")
                                        // other required and custom claims
                                   }, "TestAuthentication"));

            // controller under test
            _productController = new ProductController(mapper, _logger.Object, _mockProductService.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext { User = user }
                }
            };
        }

        [Theory]
        [InlineData("testUserId", "test@email.com")]
        public async Task GetAll_ReturnsEmptyList_WhenNoItems(string userId, string email)
        {
            // Arrange
            var userFixture = _fixture.Build<User>().With(x => x.Email, email)
                                                    .With(x => x.Id, userId)
                                                    .Create();
            var productsFixture = _fixture.Build<Product>().CreateMany(0);

            _mockUserService.Setup(x => x.GetByEmailAsync(email)).ReturnsAsync(userFixture);
            _mockProductService.Setup(x => x.GetAllAsync(email)).ReturnsAsync(productsFixture);

            // Act
            var response = (OkObjectResult)await _productController.GetAllAsync();

            // Assert
            Assert.Empty((IEnumerable<ProductDTO>)response.Value);
        }
    }
}
