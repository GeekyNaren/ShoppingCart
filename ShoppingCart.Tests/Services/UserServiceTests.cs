using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShoppingCart.Interfaces;
using ShoppingCart.Models;
using ShoppingCart.Models.Dtos;
using ShoppingCart.Models.Responses;
using ShoppingCart.Services;
using Xunit;

namespace ShoppingCart.Tests.Services
{
    public class UserServiceTests
    {
        private class InMemoryUserRepository : IUserRepository
        {
            public List<User> Items { get; } = new();
            public Task AddAsync(User entity)
            {
                Items.Add(entity);
                return Task.CompletedTask;
            }

            public Task DeleteAsync(string id)
            {
                var it = Items.FirstOrDefault(x => x.Id == id);
                if (it != null) Items.Remove(it);
                return Task.CompletedTask;
            }

            public Task<IEnumerable<User>> GetAllAsync() => Task.FromResult(Items.AsEnumerable());

            public Task<User> GetByIdAsync(string id)
            {
                var it = Items.FirstOrDefault(x => x.Id == id);
                return Task.FromResult(it);
            }

            public Task<User?> GetByEmailAsync(string email)
            {
                var it = Items.FirstOrDefault(x => x.Email == email);
                return Task.FromResult<User?>(it);
            }

            public Task<User?> GetByUsernameAsync(string username)
            {
                var it = Items.FirstOrDefault(x => x.Username == username);
                return Task.FromResult<User?>(it);
            }

            public Task UpdateAsync(string id, User entity)
            {
                var idx = Items.FindIndex(x => x.Id == id);
                if (idx >= 0) Items[idx] = entity;
                return Task.CompletedTask;
            }
        }

        [Fact]
        public async Task GetUsers_ReturnsDtos()
        {
            var repo = new InMemoryUserRepository();
            repo.Items.Add(new User { Id = "1", Username = "u1", Email = "e1@example.com", Role = "Customer", CreatedAt = DateTime.UtcNow });
            var svc = new UserService(repo);

            var res = await svc.GetUsers();

            Assert.True(res.Success);
            Assert.NotNull(res.Data);
            Assert.Single(res.Data);
            Assert.Equal("u1", res.Data[0].Username);
        }

        [Fact]
        public async Task RegisterUser_Duplicate_ReturnsFail()
        {
            var repo = new InMemoryUserRepository();
            repo.Items.Add(new User { Id = "1", Username = "dup", Email = "dup@example.com", Role = "Customer", CreatedAt = DateTime.UtcNow });
            var svc = new UserService(repo);

            var request = new ShoppingCart.Models.Dtos.RegisterRequest
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
            var repo = new InMemoryUserRepository();
            var svc = new UserService(repo);

            var request = new ShoppingCart.Models.Dtos.RegisterRequest
            {
                Username = "newuser",
                Email = "new@example.com",
                Password = "password",
                Role = "Customer"
            };

            var res = await svc.RegisterUser(request);

            Assert.True(res.Success);
            Assert.Single(repo.Items);
            Assert.Equal("newuser", repo.Items[0].Username);
            Assert.Equal("new@example.com", repo.Items[0].Email);
        }
    }
}
