using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using ShoppingCart.Interfaces;
using ShoppingCart.Models;
using ShoppingCart.Models.Dtos;
using ShoppingCart.Services;
using Xunit;

namespace ShoppingCart.Tests.Services
{
    public class UserServiceTests
    {
        [Fact]
        public async Task GetUsers_ReturnsDtos()
        {
            var users = new List<User>
            {
                new User { Id = "1", Username = "u1", Email = "e1@example.com", Role = "Customer", CreatedAt = DateTime.UtcNow }
            };

            var repoMock = new Mock<IUserRepository>();
            repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(users);

            var svc = new UserService(repoMock.Object);

            var res = await svc.GetUsers();

            Assert.True(res.Success);
            Assert.NotNull(res.Data);
            Assert.Single(res.Data);
            Assert.Equal("u1", res.Data[0].Username);
        }

        [Fact]
        public async Task RegisterUser_Duplicate_ReturnsFail()
        {
            var users = new List<User>
            {
                new User { Id = "1", Username = "dup", Email = "dup@example.com", Role = "Customer", CreatedAt = DateTime.UtcNow }
            };

            var repoMock = new Mock<IUserRepository>();
            repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(users);

            var svc = new UserService(repoMock.Object);

            var request = new RegisterRequest
            {
                Username = "dup",
                Email = "new@example.com",
                Password = "password",
                Role = "Customer"
            };

            var res = await svc.RegisterUser(request);

            Assert.False(res.Success);
        }

        [Fact]
        public async Task RegisterUser_Success_AddsUser()
        {
            var repoMock = new Mock<IUserRepository>();
            // start with no users
            repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<User>());

            var added = new List<User>();
            repoMock.Setup(r => r.AddAsync(It.IsAny<User>())).Returns<User>(u =>
            {
                added.Add(u);
                return Task.CompletedTask;
            });

            var svc = new UserService(repoMock.Object);

            var request = new RegisterRequest
            {
                Username = "newuser",
                Email = "new@example.com",
                Password = "password",
                Role = "Customer"
            };

            var res = await svc.RegisterUser(request);

            Assert.True(res.Success);
            Assert.Single(added);
            Assert.Equal("newuser", added[0].Username);
            Assert.Equal("new@example.com", added[0].Email);
        }
    }
}
