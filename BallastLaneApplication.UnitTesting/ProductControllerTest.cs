using AutoFixture;
using AutoMapper;
using BallastLaneApplication.Controllers;
using BallastLaneApplication.Data.Service.Interfaces;
using BallastLaneApplication.Domain.DTOs;
using BallastLaneApplication.Domain.Entities;
using BallastLaneApplication.Mapping;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Moq;
using System.Collections.Generic;
using System.Security.Claims;

namespace BallastLaneApplication.UnitTesting
{
    public class ProductControllerTest
    {
        private Fixture _fixture;
        private readonly ProductController _productController;
        private Mock<IProductService> _mockProductService;
        private Mock<IUserService> _mockUserService;

        public ProductControllerTest()
        {
            // fixture for creating test data
            _fixture = new Fixture();

            // automapper dependency
            var mapper = new MapperConfiguration(x => x.AddProfile<MapperConfig>()).CreateMapper();

            // mock dependencies
            _mockProductService = new Mock<IProductService>();
            _mockUserService = new Mock<IUserService>();

            // claims
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] {
                                        new Claim(ClaimTypes.NameIdentifier, "test@email.com"),
                                        new Claim(ClaimTypes.Name, "test@email.com")
                                        // other required and custom claims
                                   }, "TestAuthentication"));

            // controller under test
            _productController = new ProductController(mapper, _mockProductService.Object)
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

        /*
        [Fact]
        public async Task GetById_ReturnsNotFound_WhenItemNotFound()
        {
            // Arrange
            _repository.Clear();

            // Act
            var response = await _client.GetAsync("/todo/1");
            response.EnsureSuccessStatusCode();
            var items = await response.Content.ReadFromJsonAsync<IEnu<Product>>();

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Add_ReturnsAddedItem_WhenItemValid()
        {
            // Arrange
            _repository.Clear();
            var newItem = new TodoItem { Title = "New Item", IsCompleted = false };
            var content = new StringContent(JsonSerializer.Serialize(newItem), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/todo", content);
            response.EnsureSuccessStatusCode();
            var addedItem = await response.Content.ReadFromJson<TodoItem>();

            // Assert
            Assert.NotNull(addedItem);
            Assert.Equal(newItem.Title, addedItem.Title);
            Assert.Equal(newItem.IsCompleted, addedItem.IsCompleted);
        }

        [Fact]
        public async Task Update_ReturnsNoContent_WhenItemUpdated()
        {
            // Arrange
            _repository.Clear();
            var newItem = new TodoItem { Title = "New Item", IsCompleted = false };
            var addedItem = await _repository.AddAsync(newItem);
            addedItem.Title = "Updated Item";
            var content = new StringContent(JsonSerializer.Serialize(addedItem), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PutAsync($"/todo/{addedItem.Id}", content);

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task Update_ReturnsNotFound_WhenItemNotFound()
        {
            // Arrange
            _repository.Clear();
            var newItem = new TodoItem { Title = "New Item", IsCompleted = false };
            var content = new StringContent(JsonSerializer.Serialize(newItem), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PutAsync("/todo/1", content);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Delete_ReturnsNoContent_WhenItemDeleted()
        {
            // Arrange
            _repository.Clear();
            var newItem = new TodoItem { Title = "New Item", IsCompleted = false };
            var addedItem = await _repository.AddAsync(newItem);

            // Act
            var response = await _client.DeleteAsync($"/todo/{addedItem.Id}");

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task Delete_ReturnsNotFound_WhenItemNotFound()
        {
            // Arrange
            _repository.Clear();

            // Act
            var response = await _client.DeleteAsync("/todo/1");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }*/
    }
}
