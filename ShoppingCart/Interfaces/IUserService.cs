using ShoppingCart.Models;
using ShoppingCart.Models.Dtos;

namespace ShoppingCart.Interfaces
{
    public interface IUserService
    {
        Task<User> RegisterAsync(string username, string email, string password, string role);
        Task<List<UserDto>> GetUsers();
    }
}
