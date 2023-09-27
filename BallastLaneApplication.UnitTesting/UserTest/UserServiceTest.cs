using AutoFixture;
using BallastLaneApplication.Data.Repository.Interfaces;
using BallastLaneApplication.Data.Service;
using BallastLaneApplication.Data.Service.Interfaces;
using BallastLaneApplication.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Moq;

namespace BallastLaneApplication.UnitTesting.UserTest
{
    public class UserServiceTest
    {
        private Fixture _fixture;
        private readonly IUserService _mockUserService;
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<IConfiguration> _configuration;

        public UserServiceTest()
        {
            // fixture for creating test data
            _fixture = new Fixture();

            // mock dependencies
            _configuration = new Mock<IConfiguration>();
            _mockUserRepository = new Mock<IUserRepository>();

            _configuration.Setup(x => x.GetSection("JwtKey").ToString()).Returns("key");

            // service under test
            _mockUserService = new UserService(_configuration.Object, _mockUserRepository.Object);
        }

        [Theory]
        [InlineData("testUserId", "test@email.com")]
        public async Task GetUsersAsync_ReturnsAllUsers(string userId, string email)
        {
            // Arrange
            var usersFixture = _fixture.Build<User>().With(x => x.Email, email)
                                                    .With(x => x.Id, userId)
                                                    .CreateMany(2);

            _mockUserRepository.Setup(x => x.GetUsersAsync()).ReturnsAsync(usersFixture);

            // Act
            var users = await _mockUserService.GetUsersAsync();

            // Assert
            Assert.True(users.Count() == 2);
            Assert.Equal(usersFixture.First().Email, users.First().Email);
        }
    }
}
