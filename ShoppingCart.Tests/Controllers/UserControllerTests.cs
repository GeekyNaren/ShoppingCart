using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Controllers;
using ShoppingCart.Interfaces;
using ShoppingCart.Models.Dtos;
using ShoppingCart.Models.Responses;
using Xunit;

namespace ShoppingCart.Tests.Controllers
{
    public class UserControllerTests
    {
        private class FakeUserService : IUserService
        {
            public ServiceResponse<bool> RegisterResult { get; set; }
            public ServiceResponse<System.Collections.Generic.List<UserDto>> UsersResult { get; set; }
            public ServiceResponse<UserDto> UserByIdResult { get; set; }
            public ServiceResponse<bool> DeleteResult { get; set; }

            public Task<ServiceResponse<bool>> RegisterUser(RegisterRequest request) => Task.FromResult(RegisterResult);
            public Task<ServiceResponse<System.Collections.Generic.List<UserDto>>> GetUsers() => Task.FromResult(UsersResult);
            public Task<ServiceResponse<UserDto>> GetUserById(string userId) => Task.FromResult(UserByIdResult);
            public Task<ServiceResponse<bool>> DeleteUser(string userId) => Task.FromResult(DeleteResult);
        }

        [Fact]
        public async Task Register_ReturnsBadRequest_OnFail()
        {
            var svc = new FakeUserService { RegisterResult = ServiceResponse<bool>.Fail("bad") };
            var ctrl = new UserController(svc);

            var result = await ctrl.Register(new RegisterRequest());

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Register_ReturnsOk_OnSuccess()
        {
            var svc = new FakeUserService { RegisterResult = ServiceResponse<bool>.Ok(true) };
            var ctrl = new UserController(svc);

            var result = await ctrl.Register(new RegisterRequest());

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetUserById_ReturnsNotFound_WhenNotFound()
        {
            var svc = new FakeUserService { UserByIdResult = ServiceResponse<UserDto>.Fail("not") };
            var ctrl = new UserController(svc);

            var result = await ctrl.GetUserById("id");

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task GetUsers_ReturnsOk()
        {
            var svc = new FakeUserService { UsersResult = ServiceResponse<System.Collections.Generic.List<UserDto>>.Ok(new System.Collections.Generic.List<UserDto>()) };
            var ctrl = new UserController(svc);

            var result = await ctrl.GetUsers();

            Assert.IsType<OkObjectResult>(result);
        }
    }
}
