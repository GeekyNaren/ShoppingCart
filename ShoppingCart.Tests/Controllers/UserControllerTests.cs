using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ShoppingCart.Controllers;
using ShoppingCart.Interfaces;
using ShoppingCart.Models.Dtos;
using ShoppingCart.Models.Responses;
using Xunit;

namespace ShoppingCart.Tests.Controllers
{
    public class UserControllerTests
    {
        [Fact]
        public async Task Register_ReturnsBadRequest_OnFail()
        {
            var mock = new Mock<IUserService>();
            mock.Setup(s => s.RegisterUser(It.IsAny<RegisterRequest>())).ReturnsAsync(ServiceResponse<bool>.Fail("bad"));

            var ctrl = new UserController(mock.Object);

            var result = await ctrl.Register(new RegisterRequest());

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Register_ReturnsOk_OnSuccess()
        {
            var mock = new Mock<IUserService>();
            mock.Setup(s => s.RegisterUser(It.IsAny<RegisterRequest>())).ReturnsAsync(ServiceResponse<bool>.Ok(true));

            var ctrl = new UserController(mock.Object);

            var result = await ctrl.Register(new RegisterRequest());

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetUserById_ReturnsNotFound_WhenNotFound()
        {
            var mock = new Mock<IUserService>();
            mock.Setup(s => s.GetUserById(It.IsAny<string>())).ReturnsAsync(ServiceResponse<UserDto>.Fail("not"));

            var ctrl = new UserController(mock.Object);

            var result = await ctrl.GetUserById("id");

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task GetUsers_ReturnsOk()
        {
            var mock = new Mock<IUserService>();
            mock.Setup(s => s.GetUsers()).ReturnsAsync(ServiceResponse<System.Collections.Generic.List<UserDto>>.Ok(new System.Collections.Generic.List<UserDto>()));

            var ctrl = new UserController(mock.Object);

            var result = await ctrl.GetUsers();

            Assert.IsType<OkObjectResult>(result);
        }
    }
}
