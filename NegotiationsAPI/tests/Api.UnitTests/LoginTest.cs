using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NegotiationsAPI.Api.Dtos;
using NegotiationsAPI.Api.Controllers;
using Moq;
using Microsoft.AspNetCore.Http;
using NegotiationsAPI.Core.Interfaces;

namespace Api.UnitTests
{
    public class LoginTest
    {
        [Fact]
        public async Task Login_CallsUserService_AndReturnsToken_WhenCredentialsAreValid()
        {
            // Arrange
            var userServiceMock = new Mock<IUserService>();
            var fakeToken = "test-token";
            userServiceMock.Setup(s => s.LoginUserAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(fakeToken);

            var controller = new AccountController(userServiceMock.Object);

            var userDto = new UserDto { Email = "test@example.com", Password = "Password123" };

            // Act
            var actionResult = await controller.Login(userDto);

            // Assert
            userServiceMock.Verify(s => s.LoginUserAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once());

            var okResult = Assert.IsType<OkObjectResult>(actionResult);
            Assert.NotNull(okResult.Value);
            Assert.Equal(fakeToken, okResult.Value.GetType().GetProperty("Token").GetValue(okResult.Value, null));
        }
    }
}