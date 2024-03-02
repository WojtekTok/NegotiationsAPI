using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NegotiationsAPI.Api.Dtos;
using NegotiationsAPI.Api.Controllers;
using Moq;
using Microsoft.AspNetCore.Http;

namespace Api.UnitTests
{
    public class UnitTest1
    {
        [Fact]
        public async Task Login_ReturnsToken_WhenCredentialsAreValid()
        {
            // Arrange
            var userStoreMock = new Mock<IUserStore<IdentityUser>>();
            var userManagerMock = new Mock<UserManager<IdentityUser>>(
                userStoreMock.Object, null, null, null, null, null, null, null, null);

            var contextAccessorMock = new Mock<IHttpContextAccessor>();
            var claimsFactoryMock = new Mock<IUserClaimsPrincipalFactory<IdentityUser>>();
            var signInManagerMock = new Mock<SignInManager<IdentityUser>>(
                userManagerMock.Object,
                contextAccessorMock.Object,
                claimsFactoryMock.Object,
                null, null, null, null);

            var loggerMock = new Mock<ILogger<AccountController>>();
            var configurationMock = new Mock<IConfiguration>();

            // Setup configurationMock to return a secret for JWT signing key
            configurationMock.Setup(c => c["Jwt:Key"]).Returns("VerySecretKey123412");

            var user = new IdentityUser { UserName = "test@example.com", Email = "test@example.com" };
            userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(user);
            signInManagerMock.Setup(x => x.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>())).ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);

            var controller = new AccountController(userManagerMock.Object, signInManagerMock.Object, configurationMock.Object);

            var userDto = new UserDto { Email = "test@example.com", Password = "Password123" };

            // Act
            var result = await controller.Login(userDto);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }
    }
}