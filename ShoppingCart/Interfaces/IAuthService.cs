using ShoppingCart.ExtensionService;
using ShoppingCart.Models.Entities;

namespace ShoppingCart.Interfaces
{
    public interface IAuthService
    {
        Task<ServiceResponse<string?>> LoginAsync(UserLogin userLogin);
        string GenerateToken(UserModel user);
        Task<UserModel?> AuthenticateAsync(UserLogin userLogin);
    }
}
