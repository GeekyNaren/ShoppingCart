using ShoppingCart.ExtensionService;
using ShoppingCart.Models;

namespace ShoppingCart.Interfaces
{
    public interface IAuthService
    {
        Task<ServiceResponse<string?>> LoginAsync(UserLogin userLogin);
        string GenerateToken(UserModel user);
        Task<UserModel?> AuthenticateAsync(UserLogin userLogin);
    }
}
