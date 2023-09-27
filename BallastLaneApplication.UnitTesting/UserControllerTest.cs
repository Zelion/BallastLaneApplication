﻿using AutoFixture;
using AutoMapper;
using BallastLaneApplication.Data.Service.Interfaces;
using BallastLaneApplication.Domain.DTOs;
using BallastLaneApplication.Domain.Entities;
using BallastLaneApplication.Domain.Enums;
using BallastLaneAuth.Controllers;
using BallastLaneAuth.Mapping;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace BallastLaneApplication.UnitTesting
{
    public class UserControllerTest
    {
        private Fixture _fixture;
        private readonly UserController _userController;
        private Mock<IUserService> _mockUserService;

        public UserControllerTest()
        {
            // fixture for creating test data
            _fixture = new Fixture();

            // automapper dependency
            var mapper = new MapperConfiguration(x => x.AddProfile<MapperConfig>()).CreateMapper();

            // mock dependencies
            _mockUserService = new Mock<IUserService>();

            // controller under test
            _userController = new UserController(_mockUserService.Object, mapper);
        }

        [Theory]
        [InlineData("test@email.com")]
        public async Task Add_ReturnsAddedItem_WhenItemValid(string email)
        {
            // Arrange
            var userDtoFixture = _fixture.Build<UserDTO>().With(x => x.Email, email).Create();
            var userFixture = _fixture.Build<User>().With(x => x.Email, email).Create();

            _mockUserService.Setup(x => x.CreateUserAsync(userFixture)).ReturnsAsync(UserCreationResults.Succeed);

            // Act
            var response = (OkObjectResult)await _userController.CreateAsync(userDtoFixture);

            // Assert
            Assert.NotNull(response.Value);
            Assert.Equal(((UserDTO)response.Value).Email, userDtoFixture.Email);
        }
    }
}
