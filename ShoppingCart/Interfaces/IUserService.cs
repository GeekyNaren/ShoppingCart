using ShoppingCart.Models;
using ShoppingCart.Models.Dtos;

namespace ShoppingCart.Interfaces
{
    public interface IUserService
    {
        Task<User> RegisterUser(RegisterRequest request);
        Task<List<UserDto>> GetUsers();
    }
}
