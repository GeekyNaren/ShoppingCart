using ShoppingCart.Models;

namespace ShoppingCart.Interfaces
{
    public interface IUserService
    {
        Task<User> RegisterAsync(string username, string email, string password, string role);
    }
}
