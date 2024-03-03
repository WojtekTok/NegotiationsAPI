using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NegotiationsAPI.Api.Dtos;
using NegotiationsAPI.Api.Controllers;
using Moq;
using Microsoft.AspNetCore.Http;
using NegotiationsAPI.Core.Interfaces;
using NegotiationsAPI.Core.Enums;
using NegotiationsAPI.Infrastructure.Services;

namespace Api.UnitTests
{
    public class UserTests
    {
        [Fact]
        public async Task Login_CallsUserService_AndReturnsToken_WhenCredentialsAreValid()
        {
            // Arrange
            var userServiceMock = new Mock<IUserService>();
            var roleServiceMock = new Mock<IRoleService>();
            var fakeToken = "test-token";
            userServiceMock.Setup(s => s.LoginUserAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(fakeToken);

            var controller = new AccountController(userServiceMock.Object, roleServiceMock.Object);

            var userDto = new LoginDto { Email = "test@example.com", Password = "Password123" };

            // Act
            var actionResult = await controller.Login(userDto);

            // Assert
            userServiceMock.Verify(s => s.LoginUserAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once());

            var okResult = Assert.IsType<OkObjectResult>(actionResult);
            Assert.NotNull(okResult.Value);
            Assert.Equal(fakeToken, okResult.Value.GetType().GetProperty("Token").GetValue(okResult.Value, null));
        }

        [Fact]
        public async Task RegisterUser_AssignsRole_WhenRegistrationIsSuccessful()
        {
            // Arrange
            var userServiceMock = new Mock<IUserService>();
            var roleServiceMock = new Mock<IRoleService>();
            var registerDto = new RegisterDto { Email = "user@example.com", Password = "Password123", Role = Roles.Client };
            var expectedRole = Roles.Client;

            userServiceMock.Setup(s => s.RegisterUserAsync(registerDto.Email, registerDto.Password))
                .ReturnsAsync(IdentityResult.Success);

            var controller = new AccountController(userServiceMock.Object, roleServiceMock.Object);

            // Act
            await controller.Register(registerDto);

            // Assert
            roleServiceMock.Verify(s => s.AssignRoleToUserAsync(registerDto.Email, expectedRole), Times.Once());
        }

        [Fact]
        public async Task EnsureRolesCreatedAsync_CreatesRoles_IfTheyDoNotExist()
        {
            // Arrange
            var roleManagerMock = new Mock<RoleManager<IdentityRole>>(
                Mock.Of<IRoleStore<IdentityRole>>(), null, null, null, null);
            var roleService = new RoleService(roleManagerMock.Object, null); // UserManager nie jest potrzebny w tym teœcie

            roleManagerMock.Setup(x => x.RoleExistsAsync(It.IsAny<string>())).ReturnsAsync(false);
            roleManagerMock.Setup(x => x.CreateAsync(It.IsAny<IdentityRole>())).ReturnsAsync(IdentityResult.Success);

            // Act
            await roleService.EnsureRolesCreatedAsync();

            // Assert
            roleManagerMock.Verify(x => x.CreateAsync(It.IsAny<IdentityRole>()), Times.AtLeastOnce());
        }
    }
}